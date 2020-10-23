import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(public auth: AuthService) { }

  public authprofile: any;
  public showMetadata: boolean = false;
  public showCovidWarning: boolean = true;
  public showHorseMusic: boolean = true;

  public async ngOnInit() {
    if(environment.production == false)
    {
      this.showMetadata = true;
      this.auth.userProfile$.subscribe(res => {
        this.authprofile = res;
      });
    }
    else
    {
      this.showMetadata = false;
    }
  }
}

