import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ProfileApiService, SantaApiGetService, SantaApiPostService, SantaApiPutService } from '../services/santa-api.service';
import { MapService } from '../services/mapper.service';
import { AuthService } from '../auth/auth.service';
import { Profile, ProfileAssignment } from 'src/classes/profile';
import { MessageHistory, ClientMeta, Message } from 'src/classes/message';
import { ProfileService } from '../services/profile.service';
import { MessageApiResponse, MessageApiReadAllResponse } from 'src/classes/responseTypes';
import { ContactPanelComponent } from '../shared/contact-panel/contact-panel.component';
import { GathererService } from '../services/gatherer.service';
import { EventType } from 'src/classes/eventType';
import { InputControlComponent } from '../shared/input-control/input-control.component';
import { SurveyResponse, Survey } from 'src/classes/survey';
import { FormControl, Validators, FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public profileService: ProfileService,
    public ProfileApiService: ProfileApiService,
    public gatherer: GathererService,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiPut: SantaApiPutService,
    public auth: AuthService,
    public ApiMapper: MapService) { }

  @ViewChild(ContactPanelComponent) chatComponent: ContactPanelComponent;
  @ViewChild(InputControlComponent) inputComponent: InputControlComponent;

  public clientID: string;
  public profile: Profile = new Profile();
  public authProfile: any;

  public selectedRecipient: ProfileAssignment = new ProfileAssignment();
  public selectedHistory: MessageHistory = new MessageHistory();
  public generalHistory: MessageHistory = new MessageHistory();

  public histories: Array<MessageHistory> = [];
  public events: Array<EventType> = [];
  public surveys: Array<Survey> = [];
  public adminRecieverMeta: ClientMeta = new ClientMeta;

  public showOverlay: boolean = false;
  public showChat: boolean = false;

  public postingMessage: boolean = false;
  public puttingMessage: boolean = false;
  public softUpdating: boolean = false;
  public refreshing: boolean = false;
  public refreshingHistories: boolean = false;

  public initializing: boolean = true;
  public gettingAllHistories: boolean = false;
  public gettingGeneralHistory: boolean = false;
  public gettingSelectedHistory: boolean = false;
  public gettingProfile: boolean = false;
  public gettingClientID: boolean = false;
  public gettingAssignments: boolean = false;

  public async ngOnInit() {
    // Boolean subscribes
    this.profileService.gettingClientID.subscribe((status: boolean) => {
      this.gettingClientID = status;
    });
    this.profileService.gettingProfile.subscribe((status: boolean) => {
      this.gettingProfile = status;
    });
    this.profileService.gettingGeneralHistory.subscribe((status: boolean) => {
      this.gettingGeneralHistory = status;
    });
    this.profileService.gettingUnloadedChatHistories.subscribe((status: boolean) => {
      this.gettingAllHistories = status;
    });
    this.profileService.gettingSelectedHistory.subscribe((status: boolean) => {
      this.gettingSelectedHistory = status;
    });
    this.profileService.gettingAssignments.subscribe((status: boolean) => {
      this.gettingAssignments = status;
    });

    // ClientID subscribe
    this.profileService.clientID.subscribe((id: string) => {
      this.clientID = id;
    });

    // Profile subscribe
    this.profileService.profile.subscribe((profile: Profile) => {
      this.profile = profile;
    });

    // Profile Assignment subscribe
    this.profileService.profileAssignments.subscribe((assignments: Array<ProfileAssignment>) => {
      this.profile.assignments = assignments;
    });

    // Assignment status subscribe
    this.profileService.profile.subscribe((profile: Profile) => {
      this.profile = profile;
    });

    // Auth profile
    this.auth.userProfile$.subscribe(data => {
      this.authProfile = data;
    });

    // Chat histories subscribe
    this.profileService.unloadedChatHistories.subscribe((histories: Array<MessageHistory>) => {
      this.histories = histories;
    });

    // Selected history subscribe
    this.profileService.selectedHistory.subscribe((selectedHistory: MessageHistory) => {
      this.selectedHistory = selectedHistory;
    });

    // General history subscribe
    this.profileService.generalHistory.subscribe((generalHistory: MessageHistory) => {
      this.generalHistory = generalHistory;
    });

    // Gatherer subscribes
    this.gatherer.allSurveys.subscribe((surveyArray: Array<Survey>) => {
      this.surveys = surveyArray
    });



    this.initializing =  true;
    await this.profileService.getClientIDFromEmail(this.authProfile.email).catch(err => {console.log(err)});
    await this.profileService.getProfileByID(this.clientID);

    this.gettingAssignments = true;
    this.ProfileApiService.getProfileAssignments(this.clientID).subscribe(async (res) => {
      let assignmentArray: Array<ProfileAssignment> = [];
      for(let i = 0; i < res.length; i++)
      {
        assignmentArray.push(this.ApiMapper.mapProfileAssignment(res[i]))
      };
      this.profile.assignments = assignmentArray;
      await this.profileService.getUnloadedHistories(this.clientID);
      await this.profileService.gatherGeneralHistory(this.clientID, this.profile.clientID);
      this.gettingAssignments = false;
    }, err => {
      console.group();
      console.log(err);
      console.groupEnd();
      this.gettingAssignments = false;
    });

    await this.gatherer.gatherAllSurveys();
    await this.gatherer.gatherAllAssignmentStatuses();
    this.initializing =  false;
  }
  public async showSelectedChat(history: MessageHistory)
  {
    this.showOverlay = true;
    this.showChat = true;

    // If the history object from the event is null, get the general history
    if(history == null)
    {
      await this.profileService.getSelectedHistory(this.profile.clientID, this.profile.clientID, null);
      setTimeout(() => this.chatComponent.scrollToBottom(), 0);

    }
    else
    {
      await this.profileService.getSelectedHistory(history.conversationClient.clientID, this.profile.clientID, history.relationXrefID);
      setTimeout(() => this.chatComponent.scrollToBottom(), 0);
    }
  }
  public async hideWindow()
  {
    if(!this.chatComponent.markingRead && !this.postingMessage && !this.puttingMessage)
    {
      this.selectedHistory = new MessageHistory();
      this.manualRefreshProfile(true);
      this.showChat = false;
      this.showOverlay = false;
    }
  }
  public async send(messageResponse: MessageApiResponse)
  {
    this.postingMessage = true;

    await this.SantaApiPost.postMessage(messageResponse).toPromise();
    await this.profileService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.profile.clientID, this.selectedHistory.relationXrefID, true);
    this.inputComponent.clearForm();

    setTimeout(() => this.chatComponent.scrollToBottom(), 0);

    this.postingMessage = false;
  }
  public async readAll()
  {
    this.puttingMessage = true;

    let unreadMessages: Array<Message> = this.selectedHistory.recieverMessages.filter((message: Message) => { return message.isMessageRead == false });

    let response: MessageApiReadAllResponse = new MessageApiReadAllResponse();
    unreadMessages.forEach((message: Message) => { response.messages.push(message.chatMessageID)});

    await this.SantaApiPut.putMessageReadAll(response).toPromise().catch((err) => {console.log(err)});
    await this.profileService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.profile.clientID, this.selectedHistory.relationXrefID, true);

    setTimeout(() => this.chatComponent.scrollToBottom(), 0);

    this.puttingMessage = false;


  }
  public async softRefreshSelectedChat(isSoftUpdate: boolean)
  {
    this.softUpdating = true;
    await this.profileService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.profile.clientID, this.selectedHistory.relationXrefID, isSoftUpdate)
    this.softUpdating = false;
  }
  public async manualRefreshSelectedChat(isSoftUpdate: boolean)
  {
    this.refreshing = true;
    await this.profileService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.profile.clientID, this.selectedHistory.relationXrefID, isSoftUpdate);
    setTimeout(() => this.chatComponent.scrollToBottom(), 0);
    this.refreshing = false;
  }
  public async manualRefreshProfile(isSoftUpdate: boolean = false)
  {
    this.refreshingHistories = true;
    this.gettingAllHistories = !isSoftUpdate;
    this.ProfileApiService.getProfileAssignments(this.clientID).subscribe(async (res) => {
      let assignmentArray: Array<ProfileAssignment> = [];
      for(let i = 0; i < res.length; i++)
      {
        assignmentArray.push(this.ApiMapper.mapProfileAssignment(res[i]))
      };
      this.profile.assignments = assignmentArray;
      await this.profileService.getUnloadedHistories(this.clientID, true);
      this.gettingAllHistories = false;
      this.refreshingHistories = false;
    }, err => {
      console.group();
      console.log(err);
      console.groupEnd();
      this.gettingAssignments = false;
    });
  }
}
