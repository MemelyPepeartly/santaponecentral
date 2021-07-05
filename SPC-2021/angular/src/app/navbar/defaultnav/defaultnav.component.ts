import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { LoginComponent } from '../../home/login/login.component';
import { environment } from 'src/environments/environment';
import { AuthService } from '@auth0/auth0-angular';


@Component({
  selector: 'app-defaultnav',
  templateUrl: './defaultnav.component.html',
  styleUrls: ['./defaultnav.component.css']
})
export class DefaultnavComponent implements OnInit {

  constructor(public auth: AuthService ) { }

  public inProduction: boolean;
  public loggedIn: boolean = false;
  ngOnInit() {
    this.inProduction = environment.production;
    this.auth.isAuthenticated$.subscribe(status => {
      this.loggedIn = status;
    });
  }
}

