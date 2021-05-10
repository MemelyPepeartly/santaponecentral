import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Profile, ProfileAssignment } from 'src/classes/profile';
import { Survey, SurveyResponse } from 'src/classes/survey';
import { AssignmentStatus } from 'src/classes/client';
import { AssignmentStatusConstants } from 'src/app/shared/constants/assignmentStatusConstants.enum';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { MessageService } from 'src/app/services/api services/message.service';
import { AddMessageRequest } from 'src/classes/request-types';

@Component({
  selector: 'app-selected-recipient',
  templateUrl: './selected-recipient.component.html',
  styleUrls: ['./selected-recipient.component.css']
})
export class SelectedRecipientComponent implements OnInit {

  constructor(public gatherer: GeneralDataGathererService,
    private MessageService: MessageService) { }

  @Input() selectedRecipient: ProfileAssignment;
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
  public async setNewStatus(newAssignmentStatusEvent: AssignmentStatus)
  {
    var oldAssignmentStatus: AssignmentStatus = this.selectedRecipient.assignmentStatus;
    this.selectedRecipient.assignmentStatus = newAssignmentStatusEvent;
    this.actionTaken.emit(true);
    if(newAssignmentStatusEvent.assignmentStatusName == AssignmentStatusConstants.SHIPPING || newAssignmentStatusEvent.assignmentStatusName == AssignmentStatusConstants.COMPLETED)
    {
      let newMessage: AddMessageRequest =
      {
        messageSenderClientID: this.profile.clientID,
        messageRecieverClientID: null,
        clientRelationXrefID: this.selectedRecipient.relationXrefID,
        eventTypeID: this.selectedRecipient.recipientEvent.eventTypeID,
        messageContent: this.profile.clientNickname + ' has set this assignment from "' + oldAssignmentStatus.assignmentStatusName + '", to "' + newAssignmentStatusEvent.assignmentStatusName + '".',
        fromAdmin: false,
      };
      await this.MessageService.postMessage(newMessage).toPromise();
    }
  }
  public setClickawayLock(event)
  {
    this.clickAwayLocked = event;
  }
}
