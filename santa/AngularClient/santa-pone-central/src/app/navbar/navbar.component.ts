import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor() { }

  showUserNav = false;
  showAdminNav = false;

  ngOnInit() {
  }

  showAdmin() {
    this.showAdminNav = true;
    this.showUserNav = false;
  }
  showNewUser() {
    this.showAdminNav = false;
    this.showUserNav = false;
  }
  showExistingUser() {
    this.showAdminNav = false;
    this.showUserNav = true;
  }

}
