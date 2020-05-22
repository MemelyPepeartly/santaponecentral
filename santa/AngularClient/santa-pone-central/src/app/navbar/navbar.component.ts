import { Component, OnInit } from '@angular/core';
import { SantaApiGetService } from '../services/santaApiService.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public rest: SantaApiGetService) { }

  showUserNav = false;
  showAdminNav = false;
  showApiNav = false;
  showNewUserNav = true;

  ngOnInit() {
  }

  showAdmin() {
    this.showAdminNav = true;
    this.showUserNav = false;
    this.showApiNav = false;
    this.showNewUserNav = false;
  }
  showNewUser() {
    this.showAdminNav = false;
    this.showUserNav = false;
    this.showApiNav = false;
    this.showNewUserNav = true;
  }
  showExistingUser() {
    this.showAdminNav = false;
    this.showUserNav = true;
    this.showApiNav = false;
    this.showNewUserNav = false;
  }
  showApi() {
    this.showApiNav = true;
    this.showAdminNav = false;
    this.showUserNav = false;
    this.showNewUserNav = false;
  }
}
