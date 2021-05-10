import { Component, OnInit, Input} from '@angular/core';
import { Tag } from 'src/classes/tag';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { MapService, MapResponse } from 'src/app/services/utility services/mapper.service';
import { TagResponse } from 'src/classes/request-types';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';

@Component({
  selector: 'app-tag-control',
  templateUrl: './tag-control.component.html',
  styleUrls: ['./tag-control.component.css']
})
export class TagControlComponent implements OnInit {

  // SANTAHERE Replace with TagService come the time
  constructor(private TagService: any,
    private formBuilder: FormBuilder,
    public ResponseMapper: MapResponse,
    private gatherer: GeneralDataGathererService,
    public ApiMapper: MapService) { }

  @Input() allTags: Array<Tag> = [];

  public addTagFormGroup: FormGroup;
  public editTagFormGroup: FormGroup;

  public selectedTag: Tag;

  public postingNewTag: boolean = false;
  public updatingTagName: boolean = false;
  public deletingTag: boolean = false;

  public gatheringAllTags: boolean = false;

  // Getters for form values for ease-of-use
  get newTag() {
    var formControlObj = this.addTagFormGroup.get('newTag') as FormControl
    return formControlObj.value
  }
  get editedTagName() {
    var formControlObj = this.editTagFormGroup.get('editTag') as FormControl
    return formControlObj.value
  }

  async ngOnInit() {
    this.gatherer.gatheringAllTags.subscribe((status: boolean) => {
      this.gatheringAllTags = status;
    });
    this.constructFormGroups();
    await this.gatherer.gatherAllTags();
  }
  private constructFormGroups() {
    this.addTagFormGroup = this.formBuilder.group({
      newTag: ['', [Validators.required, Validators.maxLength(50), Validators.minLength(2), Validators.pattern("[A-Za-z0-9 ]{2,50}")]],
    });
    this.editTagFormGroup = this.formBuilder.group({
      editTag: ['', [Validators.required, Validators.maxLength(50), Validators.minLength(2), Validators.pattern("[A-Za-z0-9 ]{2,50}")]],
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
  public async addNewTag()
  {
    this.postingNewTag = true;

    let newTagResponse: TagResponse = new TagResponse();
    newTagResponse.tagName = this.newTag;

    await this.TagService.postTag(newTagResponse).toPromise().catch((err) => {console.log(err)});
    await this.gatherer.gatherAllTags();
    this.addTagFormGroup.reset();

    this.postingNewTag = false;
  }
  public async editTag()
  {
    this.updatingTagName = true;
    let updatedTag: Tag = this.selectedTag
    updatedTag.tagName = this.editedTagName
    let updatedTagResponse: TagResponse = this.ResponseMapper.mapTagResponse(updatedTag)

    await this.TagService.putTagName(this.selectedTag.tagID, updatedTagResponse).toPromise();
    await this.gatherer.gatherAllTags();
    this.editTagFormGroup.reset();

    this.updatingTagName = false;
  }
  public async deleteTag(tag: Tag)
  {
    this.deletingTag = true;

    await this.TagService.deleteTag(tag.tagID).toPromise().catch((err) => {console.log(err)});
    await this.gatherer.gatherAllTags();
    this.selectedTag = undefined;

    this.deletingTag = false;
  }
  public sortDeletable() : Array<Tag>
  {
    return this.allTags.filter((tag: Tag) => {return tag.deletable == true && tag.tagImmutable == false})
  }
  public sortEditable() : Array<Tag>
  {
    return this.allTags.filter((tag: Tag) => {return tag.tagImmutable != true})
  }
}
