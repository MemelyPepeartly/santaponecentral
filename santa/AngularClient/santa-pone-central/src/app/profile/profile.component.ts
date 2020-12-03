import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ProfileApiService, SantaApiGetService, SantaApiPostService, SantaApiPutService } from '../services/santa-api.service';
import { MapService } from '../services/mapper.service';
import { AuthService } from '../auth/auth.service';
import { Profile, ProfileAssignment } from 'src/classes/profile';
import { MessageHistory, ClientMeta, Message, ChatInfoContainer } from 'src/classes/message';
import { ProfileService } from '../services/profile.service';
import { MessageApiResponse, MessageApiReadAllResponse } from 'src/classes/responseTypes';
import { ContactPanelComponent } from '../shared/contact-panel/contact-panel.component';
import { GathererService } from '../services/gatherer.service';
import { EventType } from 'src/classes/eventType';
import { InputControlComponent } from '../shared/input-control/input-control.component';
import { SurveyResponse, Survey } from 'src/classes/survey';
import { FormControl, Validators, FormGroup, FormBuilder } from '@angular/forms';
import { ChatComponent } from '../shared/chat/chat.component';

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

  @ViewChild(ChatComponent) chatComponent: ChatComponent;

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
  public chatInfoContainer: ChatInfoContainer = new ChatInfoContainer();

  public showOverlay: boolean = false;
  public showChat: boolean = false;

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
    await this.gatherer.gatherAllSurveys();
    await this.gatherer.gatherAllAssignmentStatuses();

    this.gettingAssignments = true;
    this.ProfileApiService.getProfileAssignments(this.clientID).subscribe(async (res) => {
      let assignmentArray: Array<ProfileAssignment> = [];
      for(let i = 0; i < res.length; i++)
      {
        assignmentArray.push(this.ApiMapper.mapProfileAssignment(res[i]))
      };
      this.profile.assignments = assignmentArray;
      await this.profileService.getUnloadedHistories(this.clientID);
      this.gettingAssignments = false;
    }, err => {
      console.group();
      console.log(err);
      console.groupEnd();
      this.gettingAssignments = false;
    });
    this.gettingGeneralHistory = true;
    this.SantaApiGet.getClientMessageHistoryBySubjectIDAndXrefID(this.clientID ,this.profile.clientID, null).subscribe((res) => {
      this.generalHistory = this.ApiMapper.mapMessageHistory(res);
      this.gettingGeneralHistory = false;
    }, err => {
      this.gettingGeneralHistory = false;
      console.group();
      console.log(err);
      console.groupEnd();
    });
    this.initializing =  false;
  }
  public async showSelectedChat(history: MessageHistory)
  {
    this.showOverlay = true;
    this.chatInfoContainer =
    {
      conversationClientID: this.profile.clientID,
      messageSenderID: this.profile.clientID,
      messageRecieverID: null,
      eventTypeID: history == null ? null : history.eventType.eventTypeID,
      relationshipXrefID: history == null ? null : history.relationXrefID,
      senderIsAdmin: false,
    }
    this.showChat = true;
  }
  public async hideWindow()
  {
    console.log("Doing the thing in hide");
    this.selectedHistory = new MessageHistory();
    this.manualRefreshProfileAssignments(true);
    this.showChat = false;
    this.showOverlay = false;
  }
  public async manualRefreshProfileAssignments(isSoftUpdate: boolean = false)
  {
    this.refreshingHistories = true;
    this.gettingAllHistories = !isSoftUpdate;

    // Gets all the profile assignments
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

    await this.profileService.gatherGeneralHistory(this.clientID, this.profile.clientID, true);
  }
}
