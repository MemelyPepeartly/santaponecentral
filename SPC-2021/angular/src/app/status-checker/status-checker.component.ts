import { Component, OnInit } from '@angular/core';
import { Status } from 'src/classes/status';
import { ClientService } from '../services/api services/client.service';
import { MapService } from '../services/utility services/mapper.service';

@Component({
  selector: 'app-status-checker',
  templateUrl: './status-checker.component.html',
  styleUrls: ['./status-checker.component.css']
})
export class StatusCheckerComponent implements OnInit {

  constructor(public ClientService: ClientService, public mapper: MapService) { }

  public emailString: string = '';

  public gettingStatus: boolean = false;
  public retrievedStatus: Status = new Status();

  ngOnInit(): void {
  }
  public getStatus()
  {
    this.gettingStatus = true;

    this.ClientService.getClientStatusByEmail(this.emailString).subscribe(res => {
      this.retrievedStatus = this.mapper.mapStatus(res);
      this.gettingStatus = false;
    },err =>{
      this.gettingStatus = false;
      console.group();
      console.log("Unable to get status");
      console.groupEnd();
    });
  }
}
