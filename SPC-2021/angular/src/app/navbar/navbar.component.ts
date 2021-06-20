import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public auth: AuthService) { }

  

  profile: any;

  isAdmin: boolean = true;

  ngOnInit() {
    this.auth.isAuthenticated$
    
    this.auth.user$.subscribe(data => {
      this.profile = data;
    });
    /*
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
    */
  }
}
