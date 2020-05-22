import { Component, OnInit, Input } from '@angular/core';
import { Survey } from 'src/classes/survey';
import { Validators, FormControl, FormBuilder, FormGroup } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/santaApiService.service';
import { MapResponse, MapService } from 'src/app/services/mapService.service';
import { GathererService } from 'src/app/services/gatherer.service';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-survey-control',
  templateUrl: './survey-control.component.html',
  styleUrls: ['./survey-control.component.css']
})
export class SurveyControlComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ResponseMapper: MapResponse,
    public gatherer: GathererService,
    public ApiMapper: MapService) { }

  @Input() allSurveys: Array<Survey> = []
  
  public addSurveyFormGroup: FormGroup;
  public editSurveyFormGroup: FormGroup;


  public selectedSurvey: Survey;
  public deletableSurveys: Array<Survey> = [];
  public tagsInUse: Array<Survey> = [];
  public allClients: Array<Client> = [];

  public postingNewSurvey: boolean = false;
  public updatingSurveyName: boolean = false;
  public deletingSurvey: boolean = false;

  // Getters for form values for ease-of-use
  get newSurvey() {
    var formControlObj = this.addSurveyFormGroup.get('newSurvey') as FormControl
    return formControlObj.value
  }
  get editedSurveyName() {
    var formControlObj = this.editSurveyFormGroup.get('editSurvey') as FormControl
    return formControlObj.value
  }

  ngOnInit(): void {
    this.constructFormGroups();
  }
  private constructFormGroups() {
    this.addSurveyFormGroup = this.formBuilder.group({
      newSurvey: [null, Validators.nullValidator && Validators.pattern],
    });
    this.editSurveyFormGroup = this.formBuilder.group({
      editSurvey: [null, Validators.nullValidator && Validators.pattern],
    });
  }
  public deleteSurvey(survey: Survey)
  {

  }
  public editSurvey()
  {

  }
  public setSelectedSurvey(survey: Survey)
  {

  }
  public unsetSelectedSurvey()
  {

  }
  public addNewSurvey()
  {
    
  }
}
