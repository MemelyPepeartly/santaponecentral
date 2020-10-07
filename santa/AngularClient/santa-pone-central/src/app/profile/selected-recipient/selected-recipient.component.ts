import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Profile, ProfileRecipient } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/profile.service';
import { Survey, SurveyResponse } from 'src/classes/survey';
import { GathererService } from 'src/app/services/gatherer.service';
import { EventType } from 'src/classes/eventType';
import { AssignmentStatus } from 'src/classes/client';

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
  @Input() profile: Profile;

  @Output() actionTaken: EventEmitter<boolean> = new EventEmitter();

  public clickAwayLocked: boolean = false;

  ngOnInit(): void {
  }
  public showSurvey(survey: Survey)
  {
    // If the responses from the client have any responses for the survey and it is for the event they are assigned, return true to show that survey
    if(this.selectedRecipient.responses.some((response: SurveyResponse) => {return response.surveyID == survey.surveyID && survey.eventTypeID == this.selectedRecipient.recipientEvent.eventTypeID}))
    {
      return true;
    }
    // Else, they didn't answer any questions for it, so dont show it
    else
    {
      return false;
    }
  }
  public setNewStatus(newAssignmentStatusEvent: AssignmentStatus)
  {
    this.selectedRecipient.assignmentStatus = newAssignmentStatusEvent
    this.actionTaken.emit(true);
  }
  public setClickawayLock(event)
  {
    this.clickAwayLocked = event;
  }
}
