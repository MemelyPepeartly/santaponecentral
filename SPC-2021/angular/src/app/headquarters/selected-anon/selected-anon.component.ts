import { Component, OnInit, Input, Output, EventEmitter, AfterViewChecked } from '@angular/core';
import { AllowedAssignmentMeta, AssignmentStatus, Client, RelationshipMeta } from '../../../classes/client';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { MapService, MapResponse } from 'src/app/services/utility services/mapper.service';
import { StatusConstants } from 'src/app/shared/constants/statusConstants.enum';
import { AssignmentStatusConstants } from 'src/app/shared/constants/assignmentStatusConstants.enum';
import { Status } from 'src/classes/status';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { EventType } from 'src/classes/eventType';
import { Survey, Question, SurveyMeta } from 'src/classes/survey';
import { Tag } from 'src/classes/tag';
import { CountriesService } from 'src/app/services/utility services/countries.service';
import { ClientService } from 'src/app/services/api services/client.service';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { AddClientTagRelationshipsRequest, ClientRelationshipsRequest, DeleteClientSenderRecipientRelationshipRequest, DeleteClientTagRelationshipRequest, EditClientAddressRequest, EditClientEmailRequest, EditClientNameRequest, EditClientNicknameRequest, EditClientStatusRequest } from 'src/classes/request-types';
import { SharkTankService } from 'src/app/services/api services/shark-tank.service';


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

  constructor(private ClientService: ClientService,
    // SANTAHERE Replace with SharkTankService once we get there
    private SharkTankService: SharkTankService,
    public ApiMapper: MapService,
    public gatherer: GeneralDataGathererService,
    public responseMapper: MapResponse,
    private formBuilder: FormBuilder,
    public countryService: CountriesService) { }

  @Input() client: Client = new Client();
  @Input() clientID: string
  @Input() loadingClient: boolean = true;
  @Input() showBackCloseButtons: boolean = false;

  @Output() actionTaken: EventEmitter<any> = new EventEmitter();
  @Output() deletedAnon: EventEmitter<any> = new EventEmitter();
  @Output() setClickAwayLockEvent: EventEmitter<any> = new EventEmitter();
  @Output() backClickedEvent: EventEmitter<any> = new EventEmitter();
  @Output() closeClickedEvent: EventEmitter<any> = new EventEmitter();

  public senders: Array<RelationshipMeta> = new Array<RelationshipMeta>();
  public recipients: Array<RelationshipMeta> = new Array<RelationshipMeta>();
  public allowedAssignmentOptions: Array<AllowedAssignmentMeta> = new Array<AllowedAssignmentMeta>();
  public events: Array<EventType> = new Array<EventType>();
  public surveys: Array<Survey> = new Array<Survey>();
  public questions: Array<Question> = new Array<Question>();
  public statuses: Array<Status> = new Array<Status>();
  public assignmentStatuses: Array<AssignmentStatus> = new Array<AssignmentStatus>();
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

  public selectedRecipients: Array<AllowedAssignmentMeta> = new Array<AllowedAssignmentMeta>();
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
  public showSentPasswordResetSuccess: boolean = false;
  public creatingAuthAccountSuccess: boolean = false;

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
  public changingAddress: boolean = false;
  public changingName: boolean = false;
  public changingEmail: boolean = false;
  public deletingClient: boolean = false;
  public sendingReset: boolean = false;
  public editingResponse: boolean = false;
  public creatingAuthAccount: boolean = false;
  public deleteConfirmationOpen: boolean = false;

  /* COMPONENT GATHERING BOOLEANS */
  public gettingAnswers: boolean = true;
  public gettingEventDetails: boolean = true;
  public gatheringAllEvents: boolean = true;
  public gatheringAllMessages: boolean = true;
  public gatheringAllQuestions: boolean = true;
  public gatheringAllStatuses: boolean = true;
  public gatheringAllSurveys: boolean = true;
  public gatheringAllTags: boolean = true;
  public gatheringAllAssignmentsStatuses: boolean = true;

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
  get clientCanBeGivenAssignments()
  {
    return this.client.clientStatus.statusDescription == StatusConstants.APPROVED || this.client.clientStatus.statusDescription == StatusConstants.COMPLETED
  }

  public async ngOnInit() {
    this.initializing = true;
    this.setup();
    this.initializing = false;
  }
  public async setup()
  {
    this.gatherer.onSelectedClient = true;

    this.loadingClient = true;
    this.client = this.ApiMapper.mapClient(await this.ClientService.getClientByClientID(this.client.clientID != undefined ? this.client.clientID : this.clientID).toPromise());
    this.loadingClient = false;

    /* FORM BUILDERS */
    this.clientNicknameFormGroup = this.formBuilder.group({
      newNickname: ['', Validators.required && Validators.pattern],
    });

    this.clientAddressFormGroup = this.formBuilder.group({
      addressLine1: ['', [Validators.required, Validators.maxLength(50)]],
      addressLine2: ['', [Validators.maxLength(50)]],
      city: ['', [Validators.required, Validators.maxLength(50)]],
      state: ['', [Validators.required, Validators.maxLength(50)]],
      postalCode: ['', [Validators.required, Validators.maxLength(25)]],
      country: ['', Validators.required]
    });
    this.clientNameFormGroup = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(20)]],
      lastName: ['', [Validators.required, Validators.maxLength(20)]],
    });
    this.clientEmailFormGroup = this.formBuilder.group({
      email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]]
    });

    /* Status subscribe and gather comes first to ensure the user doesn't click the button before they are allowed, causing an error */
    this.gatherer.allStatuses.subscribe((statusArray: Array<Status>) => {
      this.statuses = statusArray;
    });
    await this.gatherer.gatherAllStatuses();

    /* ---- CLIENT SUBSCRIBES ---- */
    //Gathers all client surveys (Must come before gatherResponses)
    this.gatherer.allSurveys.subscribe((surveyArray: Array<Survey>) => {
      this.surveys = surveyArray;
    });
    
    /* ---- COMPONENT SPECIFIC GATHERS ---- */
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
    //Gather all assignment statuses
    this.gatherer.allAssignmentStatuses.subscribe((assignmentStatusArray: Array<AssignmentStatus>) => {
      this.assignmentStatuses = assignmentStatusArray;
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
    this.gatherer.gatheringAllAssignmentsStatuses.subscribe((status: boolean) => {
      this.gatheringAllAssignmentsStatuses = status;
    });

    //Runs all gather services
    await this.gatherer.gatherAllEvents();
    await this.gatherer.gatherAllQuestions();
    await this.gatherer.gatherAllSurveys();
    await this.gatherer.gatherAllTags();
    await this.gatherer.gatherAllAssignmentStatuses();

    this.gettingAnswers = false;
    this.gettingEventDetails = false;
  }
  isAnonApproved() : boolean
  {
    //Tells card if client is approved to hide or show the recipient add profile controls
    return this.client.clientID != undefined ? this.client.clientStatus.statusDescription == StatusConstants.APPROVED : false
  }
  answeredToSurvey(assignmentChoice: AllowedAssignmentMeta, survey: Survey) : boolean
  {
    return assignmentChoice.answeredSurveys.some((surveyMeta: SurveyMeta) => {return surveyMeta.surveyID == survey.surveyID && surveyMeta.eventTypeID == survey.eventTypeID})
  }
  public approveAnon(wantsAccount: boolean)
  {

    this.showButtonSpinner = true;
    var approvedStatus: Status = this.getStatusByConstant(StatusConstants.APPROVED);

    let clientStatusResponse: EditClientStatusRequest =
    {
      clientStatusID: approvedStatus.statusID,
      wantsAccount: wantsAccount
    };

    this.ClientService.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
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

    let clientStatusResponse: EditClientStatusRequest = new EditClientStatusRequest();
    clientStatusResponse.clientStatusID = deniedStatus.statusID

    this.ClientService.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
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

    let clientStatusResponse: EditClientStatusRequest = new EditClientStatusRequest();
    clientStatusResponse.clientStatusID = completedStatus.statusID


    this.ClientService.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
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

    let clientStatusResponse: EditClientStatusRequest = new EditClientStatusRequest();
    clientStatusResponse.clientStatusID = approvedStatus.statusID

    this.ClientService.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(() => {
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

    await this.ClientService.deleteClient(this.client.clientID).toPromise().catch((error) => {console.log(error)});

    this.actionTaken.emit(true);

    this.deletingClient = false;
    this.deletedAnon.emit(true);

  }
  public async sendAnonPasswordReset()
  {
    this.sendingReset = true;

    await this.SharkTankService.postPasswordResetToClient(this.client.clientID).toPromise().catch((error) => {console.log(error)});

    this.showSentPasswordResetSuccess = true;
    this.sendingReset = false;
  }
  // SANTAHERE This might be depreciated with the new client service stuff
  public createAnonAuth0Account()
  {
    this.creatingAuthAccount = true;


    this.SharkTankService.postClientNewAuth0Account(this.client.clientID).subscribe((res) => {
      this.creatingAuthAccountSuccess = true;
      this.creatingAuthAccount = false;
      this.actionTaken.emit(true);
      this.client = this.ApiMapper.mapClient(res);
    },
    err => {
      console.log(err);
      this.creatingAuthAccount = false;
      this.creatingAuthAccountSuccess = false;
    });
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
      let clientNicknameResponse: EditClientNicknameRequest = this.responseMapper.mapClientNicknameResponse(putClient);

      await this.ClientService.putClientNickname(putClient.clientID, clientNicknameResponse).toPromise();

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

    let assignments: ClientRelationshipsRequest =
    {
      eventTypeID: this.selectedRecipientEvent.eventTypeID,
      assignmentStatusID: this.assignmentStatuses.find((status: AssignmentStatus) => {return status.assignmentStatusName == AssignmentStatusConstants.ASSIGNED}).assignmentStatusID,
      assignments: []
    };

    this.selectedRecipients.forEach((selectedRecipient: AllowedAssignmentMeta) => {
      assignments.assignments.push(selectedRecipient.clientMeta.clientID);
    });

    this.client = this.ApiMapper.mapClient(await this.ClientService.postClientRecipients(this.client.clientID, assignments).toPromise().catch(err => console.log(err)));

    await this.getAllowedRecipientsByEvent(currentEvent);

    this.actionTaken.emit(true);

    this.addRecipientSuccess = true;
    this.showRecipientListPostingSpinner = false;
    this.selectedRecipients = [];
  }

  public async getAllowedRecipientsByEvent(eventType: EventType)
  {
    this.recipientsAreLoaded = false;
    this.selectedRecipientEvent = eventType;

    var response = await this.ClientService.getAllowedAssignmentsByID(this.client.clientID, this.selectedRecipientEvent.eventTypeID).toPromise();
    this.allowedAssignmentOptions = [];
    response.forEach((meta: any) => {
      this.allowedAssignmentOptions.push(this.ApiMapper.mapAllowedAssignmentMeta(meta))
    });

    this.recipientsAreLoaded=true;
  }
  public async switchAnon(anon: RelationshipMeta)
  {
    this.beingSwitched = true;
    this.addRecipientSuccess = false;
    this.recipientOpen = false;
    let switchClient: Client = this.ApiMapper.mapClient(await this.ClientService.getClientByClientID(anon.relationshipClient.clientID).toPromise());
    this.client = switchClient;

    if(this.client.clientStatus.statusDescription == StatusConstants.APPROVED)
    {
      this.clientApproved = true;
    }
    this.gatherer.gatherAllSurveys();
    this.gatherer.gatherAllEvents();
    this.beingSwitched = false;
  }
  public async removeRecipient(anon: RelationshipMeta)
  {
    this.beingRemoved = true;
    let response: DeleteClientSenderRecipientRelationshipRequest =
    {
      clientID: anon.relationshipClient.clientID,
      clientEventTypeID: anon.eventType.eventTypeID
    }
    var res = await this.ClientService.deleteClientRecipient(this.client.clientID, response).toPromise();
    this.client = this.ApiMapper.mapClient(res);

    this.actionTaken.emit(true);
    this.recipientOpen = false;
    this.selectedRecipientEvent = new EventType();
    this.softRefreshClient();

    this.beingRemoved = false;
  }
  public async removeTagFromClient(tag: Tag)
  {
    this.modyingTagRelationships = true;

    let relationship = new DeleteClientTagRelationshipRequest();
    relationship.clientID = this.client.clientID;
    relationship.tagID = tag.tagID;
    var res = await this.ClientService.deleteTagFromClient(relationship).toPromise();
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

    let clientTagRelationships: AddClientTagRelationshipsRequest = new AddClientTagRelationshipsRequest();
    this.selectedTags.forEach((tag: Tag) => {
      clientTagRelationships.tags.push(tag.tagID)
    });
    this.client = this.ApiMapper.mapClient(await this.ClientService.postTagsToClient(this.client.clientID, clientTagRelationships).toPromise());


    await this.setClientTags();
    await this.showAvailableTags();
    this.actionTaken.emit(true)
    this.modyingTagRelationships = false;
    this.editingTags = false;
  }
  public async cancelTagAction()
  {
    this.editingTags = false;
  }
  public async setClientTags()
  {
    this.currentTags = [];
    this.currentTags = this.client.tags;
  }
  public async submitNewAddress()
  {
    this.changingAddress = true;

    let newAddressResponse: EditClientAddressRequest =
    {
      clientAddressLine1: this.addressFormControls.addressLine1.value,
      clientAddressLine2: this.addressFormControls.addressLine2.value,
      clientCity: this.addressFormControls.city.value,
      clientState: this.addressFormControls.state.value,
      clientPostalCode: this.addressFormControls.postalCode.value,
      clientCountry: this.addressFormControls.country.value,
    }

    this.client = this.ApiMapper.mapClient(await this.ClientService.putClientAddress(this.client.clientID, newAddressResponse).toPromise());
    this.clientAddressFormGroup.reset();
    this.showAddressChangeForm = false;
    this.actionTaken.emit(true);

    this.changingAddress = false;
  }
  public async submitNewName()
  {
    this.changingName = true;

    let newNameResponse = new EditClientNameRequest();

    newNameResponse.clientName = this.nameFormControls.firstName.value + " " + this.nameFormControls.lastName.value;

    this.client = this.ApiMapper.mapClient(await this.ClientService.putClientName(this.client.clientID, newNameResponse).toPromise());
    this.clientNameFormGroup.reset();
    this.showNameChangeForm = false;
    this.actionTaken.emit(true);

    this.changingName = false;
  }
  public async submitNewEmail()
  {
    this.changingEmail = true;

    let newEmailResponse = new EditClientEmailRequest();

    newEmailResponse.clientEmail = this.emailFormControls.email.value;

    this.client = this.ApiMapper.mapClient(await this.ClientService.putClientEmail(this.client.clientID, newEmailResponse).toPromise());
    this.clientNameFormGroup.reset();
    this.showEmailChangeForm = false;
    this.actionTaken.emit(true);

    this.changingEmail = false;
  }
  public async softRefreshClient(emitRefresh: boolean = false)
  {
    this.client = this.ApiMapper.mapClient(await this.ClientService.getClientByClientID(this.client.clientID).toPromise());
    if(emitRefresh)
    {
      this.actionTaken.emit(true);
    }
  }
  public getEventSenders(eventType: EventType) : Array<RelationshipMeta>
  {
    return this.client.senders.filter((sender: RelationshipMeta) => {return sender.eventType.eventTypeID == eventType.eventTypeID});
  }
  public getEventAssignments(eventType: EventType) : Array<RelationshipMeta>
  {
    return this.client.assignments.filter((assignment: RelationshipMeta) => {return assignment.eventType.eventTypeID == eventType.eventTypeID});
  }
  public setClickAwayAllowed(status: boolean)
  {
    this.setClickAwayLockEvent.emit(status);
  }
  public emitActionTaken()
  {
    this.actionTaken.emit(true);
  }
  public emitBackClicked()
  {
    this.backClickedEvent.emit(true);
  }
  public emitCloseClicked()
  {
    this.closeClickedEvent.emit(true);
  }
}
