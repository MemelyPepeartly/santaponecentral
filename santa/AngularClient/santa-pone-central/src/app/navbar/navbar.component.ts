import { Component, OnInit } from '@angular/core';
import { SantaApiService } from '../services/SantaApiService.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public rest: SantaApiService) { }

  showUserNav = false;
  showAdminNav = false;
  showApiNav = false;

  ngOnInit() {
  }

  showAdmin() {
    this.showAdminNav = true;
    this.showUserNav = false;
    this.showApiNav = false;
  }
  showNewUser() {
    this.showAdminNav = false;
    this.showUserNav = false;
    this.showApiNav = false;
  }
  showExistingUser() {
    this.showAdminNav = false;
    this.showUserNav = true;
    this.showApiNav = false;
  }
  showApi() {
    this.showApiNav = true;
    this.showAdminNav = false;
    this.showUserNav = false;
  }
}
