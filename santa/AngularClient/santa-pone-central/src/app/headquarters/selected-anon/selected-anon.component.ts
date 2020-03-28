import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Client, ClientSenderRecipientRelationship } from '../../../classes/client';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService } from 'src/app/services/SantaApiService.service';
import { MapService, MapResponse } from 'src/app/services/MapService.service';
import { EventConstants } from 'src/app/shared/constants/EventConstants';
import { Status } from 'src/classes/status';
import { ClientStatusResponse, ClientNicknameResponse, ClientRelationshipResponse } from 'src/classes/responseTypes';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { EventType } from 'src/classes/EventType';

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
    public ApiMapper: MapService,
    public responseMapper: MapResponse,
    private formBuilder: FormBuilder) { }

  @Input() client: Client = new Client();
  @Output() action: EventEmitter<any> = new EventEmitter();
  @Output() refreshSelectedClient: EventEmitter<any> = new EventEmitter();

  public senders: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public recipients: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public approvedRecipientClients: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public events: Array<EventType> = new Array<EventType>();

  public selectedRecipients: Array<Client> = new Array<Client>();
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

  public clientNicknameFormGroup: FormGroup;

  ngOnInit() {
    //Tells card if client is approved to hide or show the recipient add profile controls
    if(this.client.clientStatus.statusDescription == EventConstants.APPROVED)
    {
      this.clientApproved = true;
    }

    this.gatherSenders();
    this.gatherRecipients();
    this.gatherEvents();
    
    this.clientNicknameFormGroup = this.formBuilder.group({
      newNickname: ['', Validators.nullValidator],
    });
  }
  public approveAnon()
  {
    
    this.showButtonSpinner = true;
    var putClient: Client = this.client;
    var approvedStatus: Status = new Status;

    this.SantaApiGet.getAllStatuses().subscribe(res =>{
      res.forEach(status => {
        if (status.statusDescription == EventConstants.APPROVED)
        {
          approvedStatus = this.ApiMapper.mapStatus(status);
          putClient.clientStatus.statusID = approvedStatus.statusID;
          var clientStatusResponse: ClientStatusResponse = this.responseMapper.mapClientStatusResponse(putClient)
    
          this.SantaApiPut.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(res => {
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
    });
  }
  public changeNickname()
  {
    this.showNickSpinner = true;
    var putClient: Client = this.client;
    var newNick: string = this.clientNicknameFormGroup.value.newNickname;

    putClient.clientNickname = newNick;
    var clientNicknameResponse: ClientNicknameResponse = this.responseMapper.mapClientNicknameResponse(putClient);
    this.SantaApiPut.putClientNickname(putClient.clientID, clientNicknameResponse).subscribe(res => {
      
      this.showNickSpinner = false;
      this.clientNicknameFormGroup.reset();
      this.showNicnameSuccess = true;
      this.actionTaken = true;
      this.action.emit(this.actionTaken);
    },
    err => {
      this.showNickSpinner = false;
      this.clientNicknameFormGroup.reset();
    });
  }
  async addRecipientsToClient()
  {
    let relationshipResponse: ClientRelationshipResponse = new ClientRelationshipResponse;
    
    
    this.selectedRecipients.forEach(async recievingClient => {
      relationshipResponse.eventTypeID = this.selectedRecipientEvent.eventTypeID;
      relationshipResponse.recieverClientID = recievingClient.clientID

      //console.log("Client in selected anon");
      //console.log(this.client)
      //console.log("Adding " + recievingClient.clientNickname + " to client recipient for the " + this.selectedRecipientEvent.eventDescription);

      this.client = this.ApiMapper.mapClient(await this.SantaApiPost.postClientRelation(this.client.clientID, relationshipResponse).toPromise().catch(err => {console.log(err)}));
      this.actionTaken = true;
      this.action.emit(this.actionTaken);
      this.showRecipientListPostingSpinner = false;
      this.addRecipientSuccess = true;
      this.refreshSelectedClient.emit(this.client.clientID);
      this.gatherRecipients();
      this.gatherSenders();
    }); 
    this.getAllowedRecipientsByEvent(this.selectedRecipientEvent);
  }
  async getAllowedRecipientsByEvent(eventType: EventType)
  {
    this.recipientsAreLoaded = false;
    this.selectedRecipientEvent = eventType;
    this.approvedRecipientClients = [];
    var recipientIDList = [];

    var recievingClient: Client = new Client();

    //Foreach relationship in recipients, if the relationshipID is equal to the eventTypeID, add it to an ID list of recipients already in the client profile.
    this.recipients.forEach(async relationship => {
      if(relationship.clientEventTypeID == eventType.eventTypeID)
      {
        recipientIDList.push(relationship.clientID);
      }
    });

    //Grab all the members in the DB
    var clientsInDBRes = await this.SantaApiGet.getAllClients().toPromise();

    //For all the clients in the response,
    //If the mapped client status is approved (&&)
    //the ID is not the currently selected client's ID (&&)
    //the client from DB is not in the list of the selected client's recipient ID list by event already
    //Push a new relationship into the approvedRecipientClient's list
    clientsInDBRes.forEach(client => {
      var mappedClient: Client = this.ApiMapper.mapClient(client);

      if(mappedClient.clientStatus.statusDescription == EventConstants.APPROVED && mappedClient.clientID != this.client.clientID && recipientIDList.includes(mappedClient.clientID) == false)
      {
        this.approvedRecipientClients.push(this.ApiMapper.mapClientRelationship(client, eventType.eventTypeID));
      }
    });
    this.recipientsAreLoaded=true;
  }
  async gatherRecipients()
  {
    this.recipients = [];
    //Gets all the recievers form the anon
    this.client.recipients.forEach(async reciever => {
      this.recipients.push(this.ApiMapper.mapClientRelationship(await this.SantaApiGet.getClient(reciever.recipientClientID).toPromise(), reciever.recipientEventTypeID));
    });
  }
  async gatherSenders()
  {
    this.senders = [];

    //Gets all the senders form the anon
    this.client.senders.forEach(async sender => {
      this.senders.push(this.ApiMapper.mapClientRelationship(await this.SantaApiGet.getClient(sender.senderClientID).toPromise(), sender.senderEventTypeID));
    });
  }
  gatherEvents()
  {
    this.events = [];
    //API Call for getting events
    this.SantaApiGet.getAllEvents().subscribe(res => {
      res.forEach(eventType => {
        if(eventType.active == true)
        {
          this.events.push(this.ApiMapper.mapEvent(eventType))
        }
      });
    });
  }
}
