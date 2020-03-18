import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { LoginComponent } from '../../home/login/login.component';


@Component({
  selector: 'app-defaultnav',
  templateUrl: './defaultnav.component.html',
  styleUrls: ['./defaultnav.component.css']
})
export class DefaultnavComponent implements OnInit {

  username = '';
  password = '';

  constructor( public dialog: MatDialog ) { }

  openDialog() {
    this.dialog.open(LoginComponent);
  }
  ngOnInit() {
  }
}

