import { Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { Tag } from 'src/classes/tag';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/SantaApiService.service';
import { MapService } from 'src/app/services/MapService.service';
import { TagResponse } from 'src/classes/responseTypes';

@Component({
  selector: 'app-tag-control',
  templateUrl: './tag-control.component.html',
  styleUrls: ['./tag-control.component.css']
})
export class TagControlComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ApiMapper: MapService) { }

  @Input() allTags: Array<Tag> = [];
  @Output() newTagMade: EventEmitter<boolean> = new EventEmitter();;

  public tagFormGroup: FormGroup;

  public postingNewTag: boolean = false;
  

  ngOnInit(): void {
    this.tagFormGroup = this.formBuilder.group({
      'newTag': [null, Validators.pattern],
    });
  }
  get newTag() {
    var formControlObj = this.tagFormGroup.get('newTag') as FormControl
    return formControlObj.value
  }
  public addNewTag()
  {
    let newTagResponse: TagResponse = new TagResponse();
    newTagResponse.tagName = this.newTag;
    
    //newTagResponse.tagName = this.tagFormGroup.value
    
    this.SantaApiPost.postTag(newTagResponse).subscribe(res =>
      {
        this.postingNewTag = true;
        this.newTagMade.emit(true);
        this.postingNewTag = false;
      },
      err =>
      {
        this.postingNewTag = false;
        console.log(err)
      })
  }

}
