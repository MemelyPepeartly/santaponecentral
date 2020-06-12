import { Component, OnInit } from '@angular/core';
import { Client } from 'src/classes/client';
import { SantaApiGetService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';
import { MapService } from '../services/mapService.service';
import { AuthService } from '../auth/auth.service';
import { Profile } from 'src/classes/profile';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(public gatherer: GathererService, 
    public SantaApiGet: SantaApiGetService,
    public auth: AuthService,
    public ApiMapper: MapService) { }

  public profile: Profile = new Profile();

  public async ngOnInit() {
    var data = this.auth.userProfile$.subscribe(async data => {
      this.profile = this.ApiMapper.mapProfile(await this.SantaApiGet.getProfile(data.email).toPromise());
    });
    // this.gatherer.allClients.subscribe((clientArray: Array<Client>) => {
    //   this.recipients = clientArray;
    // });
    // this.gatherer.gatherAllClients();
  }

}
