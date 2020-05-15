import { Component, OnInit, Input, Output, EventEmitter, ViewChild, AfterViewInit, OnChanges} from '@angular/core';
import { Tag } from 'src/classes/tag';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/SantaApiService.service';
import { MapService, MapResponse } from 'src/app/services/MapService.service';
import { TagResponse } from 'src/classes/responseTypes';
import { GathererService } from 'src/app/services/Gatherer.service';
import { MatChip } from '@angular/material/chips';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-tag-control',
  templateUrl: './tag-control.component.html',
  styleUrls: ['./tag-control.component.css']
})
export class TagControlComponent implements OnInit, AfterViewInit, OnChanges {

  constructor(private formBuilder: FormBuilder,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ResponseMapper: MapResponse,
    public gatherer: GathererService,
    public ApiMapper: MapService) { }

  @Input() allTags: Array<Tag> = [];

  public addTagFormGroup: FormGroup;
  public editTagFormGroup: FormGroup;


  public selectedTag: Tag;
  public deletableTags: Array<Tag> = [];
  public tagsInUse: Array<Tag> = [];
  public allClients: Array<Client> = [];

  public postingNewTag: boolean = false;
  public updatingTagName: boolean = false;
  public deletingTag: boolean = false;

  // Getters for form values for ease-of-use
  get newTag() {
    var formControlObj = this.addTagFormGroup.get('newTag') as FormControl
    return formControlObj.value
  }
  get editedTagName() {
    var formControlObj = this.editTagFormGroup.get('editTag') as FormControl
    return formControlObj.value
  }
  

  ngOnInit() {
    this.constructFormGroups();
  }
  async ngAfterViewInit() {
    this.gatherer.allClients.subscribe((clients: Array<Client>) => {
      this.allClients = clients;
    });
    await this.gatherer.gatherAllTags();
    await this.sortDeletableTags();
  }
  async ngOnChanges() {
    await this.sortDeletableTags();
  }
  private constructFormGroups() {
    this.addTagFormGroup = this.formBuilder.group({
      newTag: [null, Validators.nullValidator && Validators.pattern],
    });
    this.editTagFormGroup = this.formBuilder.group({
      editTag: [null, Validators.nullValidator && Validators.pattern],
    });
  }
  public setSelectedTag(tag: Tag)
  {
    this.selectedTag = tag;
  }
  public unsetSelectedTag()
  {
    this.selectedTag = undefined;
  }
  public addNewTag()
  {
    this.postingNewTag = true;

    let newTagResponse: TagResponse = new TagResponse();
    newTagResponse.tagName = this.newTag;
    
    this.SantaApiPost.postTag(newTagResponse).subscribe(res =>
      {
        this.gatherer.gatherAllTags();
        this.postingNewTag = false;
      },
      err =>
      {
        this.postingNewTag = false;
        console.log(err)
      })
  }
  public async editTag()
  {    
    this.updatingTagName = true;
    let updatedTag: Tag = this.selectedTag
    updatedTag.tagName = this.editedTagName
    let updatedTagResponse: TagResponse = this.ResponseMapper.mapTagResponse(updatedTag)
    
    await this.SantaApiPut.putTagName(this.selectedTag.tagID, updatedTagResponse).toPromise();
    await this.gatherer.gatherAllTags();
    this.updatingTagName = false;
  }
  public async deleteTag(tag: Tag)
  {
    this.deletingTag = true;
    this.SantaApiDelete.deleteTag(tag.tagID).subscribe(() => {
      this.gatherer.gatherAllTags();
      this.deletingTag = false;
    },
    err => {
      this.deletingTag = false;
      console.log(err); 
    });
  }
  public async sortDeletableTags()
  {
    this.deletableTags = [];
    //For all the clients
    for(let i = 0; i < this.allClients.length; i++)
    {
      //If the client has more than 0 tags
      if(this.allClients[i].tags.length > 0)
      {
        //Look through each tag
        for(let j = 0; j < this.allClients[i].tags.length; j++)
        {
          //If the tag in question doesnt already exist in the list of in-use tags
          if(!this.tagsInUse.some((tag: Tag) => tag.tagID == this.allClients[i].tags[j].tagID))
          {
            //Add the tag to the list of used tags
            this.tagsInUse.push(this.allClients[i].tags[j])
          }
        } 
      }
    }

    //For each tag available
    for(let i = 0; i < this.allTags.length; i++)
    {
      //If the list of tagsInUse does not have the current tag in it
      if(!this.tagsInUse.some((tag: Tag) => tag.tagID == this.allTags[i].tagID))
      {
        //Add it to the list of deletable tags
        this.deletableTags.push(this.allTags[i]);
      }
    }
  }
}
