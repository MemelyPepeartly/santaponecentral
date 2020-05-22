import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Client, ClientSenderRecipientRelationship } from '../../../classes/client';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/santaApiService.service';
import { MapService, MapResponse } from 'src/app/services/mapService.service';
import { EventConstants } from 'src/app/shared/constants/eventConstants';
import { Status } from 'src/classes/status';
import { ClientStatusResponse, ClientNicknameResponse, ClientRelationshipResponse, ClientTagRelationshipResponse } from 'src/classes/responseTypes';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { EventType } from 'src/classes/eventType';
import { SurveyResponse, Survey, SurveyQA, Question } from 'src/classes/survey';
import { Tag } from 'src/classes/tag';
import { BreakpointObserver } from '@angular/cdk/layout';
import { GathererService } from 'src/app/services/gatherer.service';
import { LoginComponent } from 'src/app/home/login/login.component';

@Component({
  selector: 'app-selected-anon',
  templateUrl: './selected-anon.component.html',
  styleUrls: ['./selected-anon.component.css'],
  animations: [
    // the fade-in/fade-out animation.
    trigger('simpleFadeAnimation', [

      // the "in" style determines the "resting" state of the element when it is visible.
      state('in', style({opacity: 1})),

      // fade in when created. this could also be written as transition('void => *')
      transition(':enter', [
        style({opacity: 0}),
        animate(600 )
      ]),

      // fade out when destroyed. this could also be written as transition('void => *')
      transition(':leave',
        animate(600, style({opacity: 0})))
    ])
  ]
})

export class SelectedAnonComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ApiMapper: MapService,
    public gatherer: GathererService,
    public responseMapper: MapResponse,
    private formBuilder: FormBuilder) { }

  @Input() client: Client = new Client();
  @Output() action: EventEmitter<any> = new EventEmitter();
  @Output() refreshSelectedClient: EventEmitter<any> = new EventEmitter();

  public senders: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public recipients: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public approvedRecipientClients: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public events: Array<EventType> = new Array<EventType>();
  public surveys: Array<Survey> = new Array<Survey>();
  public questions: Array<Question> = new Array<Question>();
  public responses: Array<SurveyResponse> = new Array<SurveyResponse>();
  public statuses: Array<Status> = new Array<Status>();
  public allClients: Array<Client> = new Array<Client>();

  //Tag arrays
  public allTags: Array<Tag> = new Array<Tag>();
  public availableTags: Array<Tag> = new Array<Tag>();
  public currentTags: Array<Tag> = new Array<Tag>();
  
  public selectedRecipients: Array<Client> = new Array<Client>();
  public selectedTags: Array<Tag> = new Array<Tag>();
  public selectedRecipientEvent: EventType = new EventType();

  public showButtonSpinner: boolean = false;
  public showNickSpinner: boolean = false;
  public showRecipientListPostingSpinner: boolean = false;
  
  public showApproveSuccess: boolean = false;
  public showNicnameSuccess: boolean = false;
  public addRecipientSuccess: boolean = false;

  public showFiller: boolean = false;
  public recipientOpen: boolean = false;
  public showFail: boolean = false;
  public actionTaken: boolean = false;
  public clientApproved: boolean = false;
  public recipientsAreLoaded: boolean = true;
  public nicknameInvalid: boolean = false;
  public beingSwitched: boolean = false;
  public beingRemoved: boolean = false;
  public tagRemovable: boolean = true;
  public editingTags: boolean = false;
  public modyingTagRelationships: boolean = false;
  public initializing: boolean = false;
  public gettingAnswers: boolean = true;
  public gettingEventDetails: boolean = true;
  public gatheringRecipients: boolean = false;
  public gatheringSenders: boolean = false;



  //Possibly depreciated
  public settingClientTags: boolean = false;



  public clientNicknameFormGroup: FormGroup;

  public async ngOnInit() {
    this.initializing = true;
    this.gatherer.onSelectedClient = true;
    //Tells card if client is approved to hide or show the recipient add profile controls
    if(this.client.clientStatus.statusDescription == EventConstants.APPROVED)
    {
      this.clientApproved = true;
    }
    this.clientNicknameFormGroup = this.formBuilder.group({
      newNickname: ['', Validators.required && Validators.pattern],
    });

    this.client = this.ApiMapper.mapClient(await this.SantaApiGet.getClient(this.client.clientID).toPromise());

    /* ---- CLIENT SUBSCRIBES ---- */
    //Gathers all client surveys (Must come before gatherResponses)
    this.gatherer.allSurveys.subscribe((surveyArray: Array<Survey>) => {
      this.surveys = surveyArray;
    });
    this.gatherer.allClients.subscribe((clientArray: Array<Client>) => {
      this.allClients = clientArray;
    });
    /* ---- COMPONENT SPECIFIC GATHERS ---- */
    //Gathers all client responses
    await this.gatherResponses();
    //Gathers all client senders
    await this.gatherSenders();
    //Gathers all client recipients
    await this.gatherRecipients();

    //Gathers all client tags
    await this.setClientTags();

    /* ---- GENERAL SUBSCRIBES ---- */
    //Gathers all events
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray;
    });
    //Gathers all tags
    this.gatherer.allTags.subscribe((tagArray: Array<Tag>) => {
      this.allTags = tagArray;
    });
    this.gatherer.allStatuses.subscribe((statusArray: Array<Status>) => {
      this.statuses = statusArray;
    })

    //Runs all gather services
    await this.gatherer.allGather();

    this.gettingAnswers = false;
    this.gettingEventDetails = false;

    this.initializing = false;
  }
  public approveAnon()
  {
    
    this.showButtonSpinner = true;
    var putClient: Client = this.client;
    var approvedStatus: Status = new Status;

    this.statuses.forEach(status =>
      {
        if (status.statusDescription == EventConstants.APPROVED)
        {
          approvedStatus = status;
          putClient.clientStatus.statusID = approvedStatus.statusID;
          var clientStatusResponse: ClientStatusResponse = this.responseMapper.mapClientStatusResponse(putClient);
          
          this.SantaApiPut.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
            this.showButtonSpinner = false;
            this.showApproveSuccess = true;
            this.actionTaken = true;
            this.action.emit(this.actionTaken);
          },
          err => {
            console.log(err);
            this.showButtonSpinner = false;
            this.showFail = true;
            this.actionTaken = false;
            this.action.emit(this.actionTaken);
          });
        }
      });
  }
  public async changeNickname()
  {
    if(this.clientNicknameFormGroup.value.newNickname != undefined && this.clientNicknameFormGroup.valid)
    {
      this.nicknameInvalid = false;
      this.showNickSpinner = true;
      var putClient: Client = this.client;
      var newNick: string = this.clientNicknameFormGroup.value.newNickname;

      putClient.clientNickname = newNick;
      var clientNicknameResponse: ClientNicknameResponse = this.responseMapper.mapClientNicknameResponse(putClient);
      await this.SantaApiPut.putClientNickname(putClient.clientID, clientNicknameResponse).toPromise();
      
      this.actionTaken = true;
      this.action.emit(this.actionTaken);
      this.clientNicknameFormGroup.reset();

      this.showNicnameSuccess = true;
      this.showNickSpinner = false;
    }
    else
    {
      this.nicknameInvalid = true;
    }
  }
  public async addRecipientsToClient()
  {
    this.showRecipientListPostingSpinner = true;

    let relationshipResponse: ClientRelationshipResponse = new ClientRelationshipResponse;
    var currentEvent = this.selectedRecipientEvent;
    var currentSelectedClientID = this.client.clientID;

    for (let i = 0; i < this.selectedRecipients.length; i++) {
      
      relationshipResponse.eventTypeID = this.selectedRecipientEvent.eventTypeID;
      relationshipResponse.recieverClientID = this.selectedRecipients[i].clientID

      await this.SantaApiPost.postClientRecipient(currentSelectedClientID, relationshipResponse).toPromise().catch(err => console.log(err));
      this.client = this.ApiMapper.mapClient(await this.SantaApiGet.getClient(this.client.clientID).toPromise())
      
      await this.gatherRecipients();
      await this.gatherSenders();

      this.actionTaken = true;
      this.action.emit(this.actionTaken);
    }

    await this.getAllowedRecipientsByEvent(currentEvent);
    this.addRecipientSuccess = true;
    this.showRecipientListPostingSpinner = false; 
  }

  public async getAllowedRecipientsByEvent(eventType: EventType)
  {
    this.recipientsAreLoaded = false;
    this.selectedRecipientEvent = eventType;

    this.approvedRecipientClients = [];

    let recipientList: Array<ClientSenderRecipientRelationship> = this.recipients.filter(filterByEvent)
    function filterByEvent(relationship: ClientSenderRecipientRelationship) {
      
      return (relationship.clientEventTypeID == eventType.eventTypeID); 
    } 
    let recipientIDList: Array<string> = this.relationListToIDList(recipientList)

    
    //refresh all API clients
    await this.gatherer.gatherAllClients();

    //For all the clients in the DB,
    //If the client status is approved (&&)
    //the ID is not the currently selected client's ID (&&)
    //the client from DB is not in the list of the selected client's recipient ID list by event already
    //Push a new possible relationship into the approvedRecipientClient's list

    for(let i = 0; i < this.allClients.length; i++)
    {
      if(this.allClients[i].clientStatus.statusDescription == EventConstants.APPROVED &&
        this.allClients[i].clientID != this.client.clientID &&
        !recipientIDList.includes(this.allClients[i].clientID))
      {
        
        this.approvedRecipientClients.push(this.ApiMapper.mapClientRelationship(this.allClients[i], eventType.eventTypeID))
      }
    }
    console.log(this.approvedRecipientClients);
    


    this.recipientsAreLoaded=true;
  }
  public relationListToIDList(relationList: Array<ClientSenderRecipientRelationship>): Array<string>
  {
    let IDList: Array<string> = []
    for(let i = 0; i < relationList.length; i++)
    {
      IDList.push(relationList[i].clientID);
    }
    return IDList;
  }
  public async switchAnon(anon: ClientSenderRecipientRelationship)
  {
    this.beingSwitched = true;
    this.addRecipientSuccess = false;
    this.recipientOpen = false;
    let switchClient: Client = this.ApiMapper.mapClient(await this.SantaApiGet.getClient(anon.clientID).toPromise());
    this.client = switchClient;

    if(this.client.clientStatus.statusDescription == EventConstants.APPROVED)
    {
      this.clientApproved = true;
    }
    this.gatherer.gatherAllSurveys();
    this.gatherer.gatherAllEvents();
    await this.gatherSenders();
    await this.gatherRecipients();
    await this.gatherResponses();
    this.beingSwitched = false;
  }
  public async removeRecipient(anon: ClientSenderRecipientRelationship)
  {
    this.beingRemoved = true;
    var res = await this.SantaApiDelete.deleteClientRecipient(this.client.clientID, anon).toPromise();
    this.client = this.ApiMapper.mapClient(res);
    await this.gatherSenders();
    await this.gatherRecipients();
    if(this.selectedRecipientEvent != undefined)
    {
      await this.getAllowedRecipientsByEvent(this.selectedRecipientEvent);
    }
    
    this.beingRemoved = false;
  }
  public async removeTagFromClient(tag: Tag)
  {
    this.modyingTagRelationships = true;

    let relationship = new ClientTagRelationshipResponse;
    relationship.clientID = this.client.clientID;
    relationship.tagID = tag.tagID;
    var res = await this.SantaApiDelete.deleteTagFromClient(relationship).toPromise();
    this.client = this.ApiMapper.mapClient(res);
    this.action.emit(true);

    await this.setClientTags();
    await this.showAvailableTags();

    this.editingTags = false;
    this.modyingTagRelationships = false;
  }
  public async showAvailableTags()
  {
    this.editingTags = true;
    this.availableTags = [];
    await this.setClientTags()
    
    
    for(let i = 0; i < this.allTags.length; i++)
    {
      // If the current tags do not contain the tag in all tags, add it to the list of available tags for the user to select for the anon
      if(!this.currentTags.some(tag => tag.tagID == this.allTags[i].tagID))
      {
        this.availableTags.push(this.allTags[i]);
      }
    }
    
  }
  public async addTagsToClient()
  {
    this.modyingTagRelationships = true;
    for(let i = 0; i < this.selectedTags.length; i++)
    {
      let clientTagRelationship = new ClientTagRelationshipResponse();
      clientTagRelationship.clientID = this.client.clientID;
      clientTagRelationship.tagID = this.selectedTags[i].tagID;
      this.client = this.ApiMapper.mapClient(await this.SantaApiPost.postTagToClient(clientTagRelationship).toPromise());
    }
    await this.setClientTags();
    await this.showAvailableTags();
    this.modyingTagRelationships = false;
    this.editingTags = false;
  }
  public async cancelTagAction()
  {
    this.editingTags = false;
  }
  public async setClientTags()
  {
    this.settingClientTags = true;

    this.currentTags = [];
    this.currentTags = this.client.tags;

    this.settingClientTags = false;
  }
  public async gatherRecipients()
  {
    this.gatheringRecipients = true
    this.recipients = [];
    //Gets all the recievers form the anon
    for(let i = 0; i < this.client.recipients.length; i++)
    {
      var foundClient = this.ApiMapper.mapClient(await this.SantaApiGet.getClient(this.client.recipients[i].recipientClientID).toPromise());
      this.recipients.push(this.ApiMapper.mapClientRelationship(foundClient ,this.client.recipients[i].recipientEventTypeID));
    }
    this.gatheringRecipients = false;
  }
  public async gatherSenders()
  {
    this.gatheringSenders = true;
    this.senders = [];

    //Gets all the senders form the anon
    for(let i = 0; i < this.client.senders.length; i++)
    {
      let foundClient: Client = this.ApiMapper.mapClient(await this.SantaApiGet.getClient(this.client.senders[i].senderClientID).toPromise());
      this.senders.push(this.ApiMapper.mapClientRelationship(foundClient ,this.client.senders[i].senderEventTypeID));
    }
    this.gatheringSenders = false;
  }
  public async gatherResponses()
  {
    this.gettingAnswers = true;
    this.responses = [];

    //API call for getting responses
    this.SantaApiGet.getSurveyResponseByClientID(this.client.clientID).subscribe(res => {
      for(let i =0; i< res.length; i++)
      {
        var mappedAnswer = this.ApiMapper.mapResponse(res[i]);

        for(let j =0; j< this.surveys.length; j++)
        {
          //If a survey in the list matches the ID of an answer's surveyID, set the eventType as the right ID it's from
          if(mappedAnswer.surveyID == this.surveys[j].surveyID)
          {
            mappedAnswer.eventTypeID = this.surveys[j].eventTypeID;

            //For each question answered, populate the actual question text
            for(let k =0; k< this.surveys[j].surveyQuestions.length; k++)
            {
              if(mappedAnswer.surveyQuestionID == this.surveys[j].surveyQuestions[k].questionID)
              {
                mappedAnswer.questionText = this.surveys[j].surveyQuestions[k].questionText;
              }
            }
          }
        }
        this.responses.push(mappedAnswer);
      }
    });
    this.gettingAnswers = false;
  }
}
