import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Client } from '../../../classes/client';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService, SantaApiPutService } from 'src/app/services/SantaApiService.service';
import { MapService, MapResponse } from 'src/app/services/MapService.service';
import { EventConstants } from 'src/app/shared/constants/EventConstants';
import { Status } from 'src/classes/status';
import { ClientStatusResponse, ClientNicknameResponse } from 'src/classes/responseTypes';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

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
    public ApiMapper: MapService,
    public responseMapper: MapResponse,
    private formBuilder: FormBuilder) { }

  @Input() client: Client = new Client();
  @Output() action: EventEmitter<any> = new EventEmitter();

  public senders: Array<Client> = new Array<Client>();
  public recievers: Array<Client> = new Array<Client>();

  public showButtonSpinner: boolean = false;
  public showNickSpinner: boolean = false;
  public showApproveSuccess: boolean = false;
  public showNicnameSuccess: boolean = false;
  public showFiller: boolean = false;
  public recipientOpen: boolean = false;
  public showFail: boolean = false;
  public actionTaken: boolean = false;

  public clientNicknameFormGroup: FormGroup;

  ngOnInit() {
    this.client.senders.forEach(clientID => {
      this.SantaApiGet.getClient(clientID).subscribe(client => {
        var c = this.ApiMapper.mapClient(client); 
        this.senders.push(c);
      });
    });
    this.client.recipients.forEach(clientID => {
      this.SantaApiGet.getClient(clientID).subscribe(client => {
        var c = this.ApiMapper.mapClient(client);
        this.recievers.push(c);
      });
    });
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
    },
    err => {
      this.showNickSpinner = false;
      this.clientNicknameFormGroup.reset();
    });
  }
}
