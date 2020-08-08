import { Component, OnInit, Input, Output, EventEmitter, AfterViewChecked } from '@angular/core';
import { Client, ClientSenderRecipientRelationship } from '../../../classes/client';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/santaApiService.service';
import { MapService, MapResponse } from 'src/app/services/mapService.service';
import { StatusConstants } from 'src/app/shared/constants/statusConstants.enum';
import { Status } from 'src/classes/status';
import { ClientStatusResponse, ClientNicknameResponse, ClientTagRelationshipResponse, ClientAddressResponse, ClientNameResponse, ClientEmailResponse, ClientRelationshipsResponse, RecipientCompletionResponse, ClientTagRelationshipsResponse} from 'src/classes/responseTypes';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { EventType } from 'src/classes/eventType';
import { Survey, Question } from 'src/classes/survey';
import { Tag } from 'src/classes/tag';
import { GathererService } from 'src/app/services/gatherer.service';
import { CountriesService } from 'src/app/services/countries.service';

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
    private formBuilder: FormBuilder,
    public countryService: CountriesService) { }

  @Input() client: Client = new Client();
  @Input() loadingClient: boolean = true;

  @Output() actionTaken: EventEmitter<any> = new EventEmitter();
  @Output() deletedAnon: EventEmitter<any> = new EventEmitter();
  @Output() refreshSelectedClient: EventEmitter<any> = new EventEmitter();

  public senders: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public recipients: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public approvedRecipientClients: Array<ClientSenderRecipientRelationship> = new Array<ClientSenderRecipientRelationship>();
  public events: Array<EventType> = new Array<EventType>();
  public surveys: Array<Survey> = new Array<Survey>();
  public questions: Array<Question> = new Array<Question>();
  public statuses: Array<Status> = new Array<Status>();
  public allClients: Array<Client> = new Array<Client>();
  public countries: Array<any> = [];

  // Forms
  public clientNicknameFormGroup: FormGroup;
  public clientAddressFormGroup: FormGroup;
  public clientNameFormGroup: FormGroup;
  public clientEmailFormGroup: FormGroup;

  public showAddressChangeForm: boolean = false;
  public showNameChangeForm: boolean = false;
  public showEmailChangeForm: boolean = false;

  // Tag arrays
  public allTags: Array<Tag> = new Array<Tag>();
  public availableTags: Array<Tag> = new Array<Tag>();
  public currentTags: Array<Tag> = new Array<Tag>();

  public selectedRecipients: Array<Client> = new Array<Client>();
  public selectedTags: Array<Tag> = new Array<Tag>();
  public selectedRecipientEvent: EventType = new EventType();

  /* SHOW SPINNER BOOLEANS */
  public showButtonSpinner: boolean = false;
  public showNickSpinner: boolean = false;
  public showRecipientListPostingSpinner: boolean = false;

  /* SUCCESS BOOLEANS */
  public showApproveSuccess: boolean = false;
  public showDeniedSuccess: boolean = false;
  public showCompletedSuccess: boolean = false;
  public showReenlistedSuccess: boolean = false;
  public showNicnameSuccess: boolean = false;
  public addRecipientSuccess: boolean = false;

  /* FUNCTIONAL COMPONENT BOOLEANS */
  public showFiller: boolean = false;
  public recipientOpen: boolean = false;
  public showFail: boolean = false;
  public clientApproved: boolean = false;
  public recipientsAreLoaded: boolean = true;
  public nicknameInvalid: boolean = false;
  public beingSwitched: boolean = false;
  public beingRemoved: boolean = false;
  public tagRemovable: boolean = true;
  public editingTags: boolean = false;
  public modyingTagRelationships: boolean = false;
  public initializing: boolean = false;
  public markingAsComplete: boolean = false;
  public changingAddress: boolean = false;
  public changingName: boolean = false;
  public changingEmail: boolean = false;
  public deletingClient: boolean = false;
  public sendingReset: boolean = false;

  /* COMPONENT GATHERING BOOLEANS */
  public gettingAnswers: boolean = true;
  public gettingEventDetails: boolean = true;
  public gatheringRecipients: boolean = false;
  public gatheringSenders: boolean = false;
  public gatheringAllEvents: boolean = true;
  public gatheringAllMessages: boolean = true;
  public gatheringAllQuestions: boolean = true;
  public gatheringAllStatuses: boolean = true;
  public gatheringAllSurveys: boolean = true;
  public gatheringAllTags: boolean = true;

  //Possibly depreciated
  public settingClientTags: boolean = false;

  get addressFormControls()
  {
    return this.clientAddressFormGroup.controls;
  }
  get nameFormControls()
  {
    return this.clientNameFormGroup.controls;
  }
  get emailFormControls()
  {
    return this.clientEmailFormGroup.controls;
  }

  public async ngOnInit() {
    this.initializing = true;
    if(this.loadingClient == false && this.client.clientID != undefined)
    {
      this.setup();
    }
    this.initializing = false;
  }
  public async setup()
  {
    this.gatherer.onSelectedClient = true;

    /* FORM BUILDERS */
    this.clientNicknameFormGroup = this.formBuilder.group({
      newNickname: ['', Validators.required && Validators.pattern],
    });

    this.clientAddressFormGroup = this.formBuilder.group({
      addressLine1: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      addressLine2: ['', [Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      city: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      state: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      postalCode: ['', [Validators.required, Validators.pattern("[0-9]{1,25}"), Validators.maxLength(25)]],
      country: ['', Validators.required]
    });
    this.clientNameFormGroup = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(20), Validators.pattern("[A-Za-z]{1,20}")]],
      lastName: ['', [Validators.required, Validators.maxLength(20), Validators.pattern("[A-Za-z]{1,20}")]],
    });
    this.clientEmailFormGroup = this.formBuilder.group({
      email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]]
    });

    /* Status subscribe and gather comes first to ensure the user doesn't click the button before they are allowed, causing an error */
    this.gatherer.allStatuses.subscribe((statusArray: Array<Status>) => {
      this.statuses = statusArray;
    });
    await this.gatherer.gatherAllStatuses();

    this.client = this.ApiMapper.mapClient(await this.SantaApiGet.getClientByClientID(this.client.clientID).toPromise());

    /* ---- CLIENT SUBSCRIBES ---- */
    //Gathers all client surveys (Must come before gatherResponses)
    this.gatherer.allSurveys.subscribe((surveyArray: Array<Survey>) => {
      this.surveys = surveyArray;
    });
    this.gatherer.allClients.subscribe((clientArray: Array<Client>) => {
      this.allClients = clientArray;
    });
    /* ---- COMPONENT SPECIFIC GATHERS ---- */
    //Gathers all client senders
    await this.gatherSenders();
    //Gathers all client recipients
    await this.gatherRecipients();
    //Gathers all client tags
    await this.setClientTags();
    //Gathers all countries for the form;
    this.countries = this.countryService.allCountries();

    /* ---- GENERAL SUBSCRIBES ---- */
    //Gathers all events
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray;
    });
    //Gathers all tags
    this.gatherer.allTags.subscribe((tagArray: Array<Tag>) => {
      this.allTags = tagArray;
    });

    /* BOOLEAN STATUS SUSCRIBES */
    this.gatherer.gatheringAllEvents.subscribe((status: boolean) => {
      this.gatheringAllEvents = status;
    });
    this.gatherer.gatheringAllMessages.subscribe((status: boolean) => {
      this.gatheringAllMessages = status;
    });
    this.gatherer.gatheringAllQuestions.subscribe((status: boolean) => {
      this.gatheringAllQuestions = status;
    });
    this.gatherer.gatheringAllStatuses.subscribe((status: boolean) => {
      this.gatheringAllStatuses = status;
    });
    this.gatherer.gatheringAllSurveys.subscribe((status: boolean) => {
      this.gatheringAllSurveys = status;
    });
    this.gatherer.gatheringAllTags.subscribe((status: boolean) => {
      this.gatheringAllTags = status;
    });

    //Runs all gather services
    await this.gatherer.gatherAllEvents();
    await this.gatherer.gatherAllQuestions();
    await this.gatherer.gatherAllSurveys();
    await this.gatherer.gatherAllTags();


    this.gettingAnswers = false;
    this.gettingEventDetails = false;
  }
  isAnonApproved() : boolean
  {
    //Tells card if client is approved to hide or show the recipient add profile controls
    return this.client.clientID != undefined ? this.client.clientStatus.statusDescription == StatusConstants.APPROVED : false
  }
  public approveAnon()
  {

    this.showButtonSpinner = true;
    var approvedStatus: Status = this.getStatusByConstant(StatusConstants.APPROVED);

    let clientStatusResponse: ClientStatusResponse = new ClientStatusResponse();
    clientStatusResponse.clientStatusID = approvedStatus.statusID

    this.SantaApiPut.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
      this.showButtonSpinner = false;
      this.showApproveSuccess = true;
      this.actionTaken.emit(true);
    },
    err => {
      console.log(err);
      this.showButtonSpinner = false;
      this.showFail = true;
      this.actionTaken.emit(false);
    });

  }
  denyAnon()
  {
    this.showButtonSpinner = true;
    var deniedStatus: Status = this.getStatusByConstant(StatusConstants.DENIED);

    let clientStatusResponse: ClientStatusResponse = new ClientStatusResponse();
    clientStatusResponse.clientStatusID = deniedStatus.statusID

    this.SantaApiPut.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
      this.showButtonSpinner = false;
      this.showDeniedSuccess = true;
      this.actionTaken.emit(true);
    },
    err => {
      console.log(err);
      this.showButtonSpinner = false;
      this.showFail = true;
      this.actionTaken.emit(false);
    });
  }
  public async setAsCompleted()
  {
    this.showButtonSpinner = true;
    var completedStatus: Status = this.getStatusByConstant(StatusConstants.COMPLETED);

    let clientStatusResponse: ClientStatusResponse = new ClientStatusResponse();
    clientStatusResponse.clientStatusID = completedStatus.statusID


    this.SantaApiPut.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
      this.showButtonSpinner = false;
      this.showCompletedSuccess = true;
      this.actionTaken.emit(true);
    },
    err => {
      console.log(err);
      this.showButtonSpinner = false;
      this.showFail = true;
      this.actionTaken.emit(false);
    });
  }
  public reenlistAnon()
  {

    this.showButtonSpinner = true;
    var approvedStatus: Status = this.getStatusByConstant(StatusConstants.APPROVED);

    let clientStatusResponse: ClientStatusResponse = new ClientStatusResponse();
    clientStatusResponse.clientStatusID = approvedStatus.statusID

    this.SantaApiPut.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
      this.showButtonSpinner = false;
      this.showReenlistedSuccess = true;
      this.actionTaken.emit(true);
    },
    err => {
      console.log(err);
      this.showButtonSpinner = false;
      this.showFail = true;
      this.actionTaken.emit(false);
    });

  }
  public async deleteAnon()
  {
    this.deletingClient = true;

    await this.SantaApiDelete.deleteClient(this.client.clientID).toPromise().catch((error) => {console.log(error)});

    this.actionTaken.emit(true);

    this.deletingClient = false;
    this.deletedAnon.emit(true);

  }
  public async sendAnonPasswordReset()
  {
    this.sendingReset = true;

    this.SantaApiPost.postPasswordResetToClient(this.client.clientID).toPromise().catch((error) => {console.log(error)});

    this.sendingReset = false;
  }
  public getStatusByConstant(statusConstant: StatusConstants) : Status
  {
    return this.statuses.find((status: Status) => {
      return status.statusDescription == statusConstant
    })
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

      this.actionTaken.emit(true);
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

    var currentEvent = this.selectedRecipientEvent;

    let assignments = new ClientRelationshipsResponse();
    assignments.eventTypeID = this.selectedRecipientEvent.eventTypeID;

    this.selectedRecipients.forEach((selectedRecipient: Client) => {
      assignments.assignments.push(selectedRecipient.clientID);
    })

    this.client = this.ApiMapper.mapClient(await this.SantaApiPost.postClientRecipients(this.client.clientID, assignments).toPromise().catch(err => console.log(err)));

    await this.gatherRecipients();
    await this.gatherSenders();
    await this.getAllowedRecipientsByEvent(currentEvent);

    this.actionTaken.emit(true);

    this.addRecipientSuccess = true;
    this.showRecipientListPostingSpinner = false;
  }

  public async getAllowedRecipientsByEvent(eventType: EventType)
  {
    this.recipientsAreLoaded = false;
    this.selectedRecipientEvent = eventType;

    this.approvedRecipientClients = [];

    let recipientList: Array<ClientSenderRecipientRelationship> = this.recipients.filter((relationship: ClientSenderRecipientRelationship) => {return (relationship.clientEventTypeID == eventType.eventTypeID)})
    let recipientIDList: Array<string> = this.relationListToIDList(recipientList)


    //refresh all API clients
    //await this.gatherer.gatherAllClients();

    //For all the clients in the DB,
    //If the client status is approved (&&)
    //the ID is not the currently selected client's ID (&&)
    //the client from DB is not in the list of the selected client's recipient ID list by event already
    //Push a new possible relationship into the approvedRecipientClient's list

    for(let i = 0; i < this.allClients.length; i++)
    {
      if(this.allClients[i].clientStatus.statusDescription == StatusConstants.APPROVED &&
        this.allClients[i].clientID != this.client.clientID &&
        !recipientIDList.includes(this.allClients[i].clientID))
      {
        this.approvedRecipientClients.push(this.mapAllowedClientRelationship(this.allClients[i], eventType.eventTypeID))
      }
    }

    this.recipientsAreLoaded=true;
  }
  public mapAllowedClientRelationship(client: Client, eventID: string)
  {
    // Might need to be revisited for removal purposes or something I dunno. Really only used in Selected Anons component
    let mappedRelationship = new ClientSenderRecipientRelationship;

    mappedRelationship.clientID = client.clientID;
    mappedRelationship.clientName = client.clientName;
    mappedRelationship.clientNickname = client.clientNickname;
    mappedRelationship.clientEventTypeID = eventID;
    mappedRelationship.removable

    return mappedRelationship;
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
    let switchClient: Client = this.ApiMapper.mapClient(await this.SantaApiGet.getClientByClientID(anon.clientID).toPromise());
    this.client = switchClient;

    if(this.client.clientStatus.statusDescription == StatusConstants.APPROVED)
    {
      this.clientApproved = true;
    }
    this.gatherer.gatherAllSurveys();
    this.gatherer.gatherAllEvents();
    await this.gatherSenders();
    await this.gatherRecipients();
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
  public async markAsComplete(anon: ClientSenderRecipientRelationship)
  {
    this.markingAsComplete = true;

    let response = new RecipientCompletionResponse();

    response.completed = true;
    response.eventTypeID = anon.clientEventTypeID;
    response.recipientID = anon.clientID;
    console.log("Client ID: " + this.client.clientID);
    console.log("Recipient ID: " + anon.clientID);

    this.client = this.ApiMapper.mapClient(await this.SantaApiPut.putClientRelationshipCompletionStatus(this.client.clientID, response).toPromise());

    await this.gatherSenders();
    await this.gatherRecipients();

    this.markingAsComplete = false;
  }
  public async removeTagFromClient(tag: Tag)
  {
    this.modyingTagRelationships = true;

    let relationship = new ClientTagRelationshipResponse;
    relationship.clientID = this.client.clientID;
    relationship.tagID = tag.tagID;
    var res = await this.SantaApiDelete.deleteTagFromClient(relationship).toPromise();
    this.client = this.ApiMapper.mapClient(res);
    this.actionTaken.emit(true);

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

    let clientTagRelationships: ClientTagRelationshipsResponse = new ClientTagRelationshipsResponse();
    this.selectedTags.forEach((tag: Tag) => {
      clientTagRelationships.tags.push(tag.tagID)
    });
    this.client = this.ApiMapper.mapClient(await this.SantaApiPost.postTagsToClient(this.client.clientID, clientTagRelationships).toPromise());


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
      let foundClient: Client = this.ApiMapper.mapClient(await this.SantaApiGet.getClientByClientID(this.client.recipients[i].recipientClientID).toPromise());
      this.recipients.push(this.ApiMapper.mapClientRecipientRelationship(foundClient ,this.client.recipients[i]));
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
      let foundClient: Client = this.ApiMapper.mapClient(await this.SantaApiGet.getClientByClientID(this.client.senders[i].senderClientID).toPromise());
      this.senders.push(this.ApiMapper.mapClientSenderRelationship(foundClient , this.client.senders[i]));
    }
    this.gatheringSenders = false;
  }
  public async submitNewAddress()
  {
    this.changingAddress = true;

    let newAddressResponse = new ClientAddressResponse();

    newAddressResponse.clientAddressLine1 = this.addressFormControls.addressLine1.value;
    newAddressResponse.clientAddressLine2 = this.addressFormControls.addressLine2.value;
    newAddressResponse.clientCity = this.addressFormControls.city.value;
    newAddressResponse.clientState = this.addressFormControls.state.value;
    newAddressResponse.clientCountry = this.addressFormControls.country.value;
    newAddressResponse.clientPostalCode = this.addressFormControls.postalCode.value;

    this.client = this.ApiMapper.mapClient(await this.SantaApiPut.putClientAddress(this.client.clientID, newAddressResponse).toPromise());
    this.clientAddressFormGroup.reset();
    this.showAddressChangeForm = false;
    this.actionTaken.emit(true);

    this.changingAddress = false;
  }
  public async submitNewName()
  {
    this.changingName = true;

    let newNameResponse = new ClientNameResponse();

    newNameResponse.clientName = this.nameFormControls.firstName.value + " " + this.nameFormControls.lastName.value;

    this.client = this.ApiMapper.mapClient(await this.SantaApiPut.putClientName(this.client.clientID, newNameResponse).toPromise());
    this.clientNameFormGroup.reset();
    this.showNameChangeForm = false;
    this.actionTaken.emit(true);

    this.changingName = false;
  }
  public async submitNewEmail()
  {
    this.changingEmail = true;

    let newEmailResponse = new ClientEmailResponse();

    newEmailResponse.clientEmail = this.emailFormControls.email.value;

    this.client = this.ApiMapper.mapClient(await this.SantaApiPut.putClientEmail(this.client.clientID, newEmailResponse).toPromise());
    this.clientNameFormGroup.reset();
    this.showEmailChangeForm = false;
    this.actionTaken.emit(true);

    this.changingEmail = false;
  }
}
