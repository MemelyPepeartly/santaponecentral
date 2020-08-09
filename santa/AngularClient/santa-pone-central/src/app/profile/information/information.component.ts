import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Profile } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/Profile.service';
import { EventType } from 'src/classes/eventType';
import { ChangeSurveyResponseModel } from 'src/classes/responseTypes';
import { SantaApiPutService } from 'src/app/services/santaApiService.service';
import { SurveyResponse } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {

  constructor(public profileService: ProfileService, public SantaApiPut: SantaApiPutService, private formBuilder: FormBuilder) { }

  @Input() loading: boolean;
  @Input() profile: Profile;
  @Input() events: Array<EventType>;
  @Input() clientResponseFormGroup: FormGroup;

  public editingResponse: boolean;


  ngOnInit(): void {
  }
  public async submitNewResponse(surveyResponseID: string)
  {
    this.editingResponse = true;

    let editedResponse = new ChangeSurveyResponseModel();
    editedResponse.responseText = this.clientResponseFormGroup.get(surveyResponseID).value
    await this.SantaApiPut.putResponse(surveyResponseID, editedResponse).toPromise();
    await this.profileService.getProfile(this.profile.email);

    this.editingResponse = false;
  }

}
