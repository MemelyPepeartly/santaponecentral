import { Component, OnInit, Input } from '@angular/core';
import { ProfileRecipient } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/Profile.service';
import { Survey, SurveyResponse } from 'src/classes/survey';
import { GathererService } from 'src/app/services/gatherer.service';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-selected-recipient',
  templateUrl: './selected-recipient.component.html',
  styleUrls: ['./selected-recipient.component.css']
})
export class SelectedRecipientComponent implements OnInit {

  constructor(public profileService: ProfileService,
    public gatherer: GathererService) { }

  @Input() selectedRecipient: ProfileRecipient;
  @Input() surveys: Array<Survey>;

  ngOnInit(): void {
  }
  public showSurvey(survey: Survey)
  {
    // If the responses from the client have any responses for the survey, return true to show that survey
    if(this.selectedRecipient.responses.some((response: SurveyResponse) => {return response.surveyID == survey.surveyID}))
    {
      return true;
    }
    // Else, they didn't answer any questions for it, so dont show it
    else
    {
      return false;
    }
  }
}
