import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public auth: AuthService) { }

  

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
