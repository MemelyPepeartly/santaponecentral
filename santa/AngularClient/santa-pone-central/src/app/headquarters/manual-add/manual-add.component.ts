import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { SantaApiGetService, SantaApiPostService } from 'src/app/services/santaApiService.service';
import { GathererService } from 'src/app/services/gatherer.service';
import { MapService, MapResponse } from 'src/app/services/mapService.service';
import { CountriesService } from 'src/app/services/countries.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SurveyFormComponent } from 'src/app/signup/survey-form/survey-form.component';

@Component({
  selector: 'app-manual-add',
  templateUrl: './manual-add.component.html',
  styleUrls: ['./manual-add.component.css']
})
export class ManualAddComponent implements OnInit {

  constructor(public SantaGet: SantaApiGetService,
    public SantaPost: SantaApiPostService,
    public gatherer: GathererService,
    public mapper: MapService,
    public responseMapper: MapResponse,
    public countryService: CountriesService,
    private formBuilder: FormBuilder) { }

  public clientInfoFormGroup: FormGroup;
  public clientAddressFormGroup: FormGroup;
  public clientEventFormGroup: FormGroup;
  public surveyFormGroup: FormGroup;

  @ViewChildren(SurveyFormComponent) surveyForms: QueryList<SurveyFormComponent>;

  ngOnInit(): void {
  }

}
