import { Component, OnInit } from '@angular/core';
import { SantaApiGetService } from '../services/SantaApiService.service';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public rest: SantaApiGetService, public auth: AuthService) { }

  showUserNav = false;
  showAdminNav = false;
  showNewUserNav = true;

  ngOnInit() {
  }

  showAdmin() {
    this.showAdminNav = true;
    this.showUserNav = false;
    this.showNewUserNav = false;
  }
  showNewUser() {
    this.showAdminNav = false;
    this.showUserNav = false;
    this.showNewUserNav = true;
  }
  showExistingUser() {
    this.showAdminNav = false;
    this.showUserNav = true;
    this.showNewUserNav = false;
  }
}
