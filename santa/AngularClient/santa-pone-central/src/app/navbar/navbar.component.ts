import { Component, OnInit } from '@angular/core';
import { SantaApiGetService } from '../services/santaApiService.service';
import { AuthService } from '../auth/auth.service';
import { RoleConstants } from '../shared/constants/roleConstants.enum';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public rest: SantaApiGetService, public auth: AuthService) { }

  

  profile: any;

  isAdmin: boolean = false;

  ngOnInit() {
    this.auth.userProfile$.subscribe(data => {
      this.profile = data;
    });
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
  }
}
