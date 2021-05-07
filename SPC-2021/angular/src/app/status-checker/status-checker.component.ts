import { Component, OnInit } from '@angular/core';
import { Status } from 'src/classes/status';
import { MapService } from '../services/utility services/mapper.service';
import { SantaApiGetService } from '../services/santa-api.service';

@Component({
  selector: 'app-status-checker',
  templateUrl: './status-checker.component.html',
  styleUrls: ['./status-checker.component.css']
})
export class StatusCheckerComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService, public mapper: MapService) { }

  public emailString: string = '';

  public gettingStatus: boolean = false;
  public retrievedStatus: Status = new Status();

  ngOnInit(): void {
  }
  public getStatus()
  {
    this.gettingStatus = true;

    this.SantaApiGet.getClientStatusByEmail(this.emailString).subscribe(res => {
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
