import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { LoginComponent } from '../../home/login/login.component';

export interface DialogData {
  username: string;
  password: string;
}

@Component({
  selector: 'app-defaultnav',
  templateUrl: './defaultnav.component.html',
  styleUrls: ['./defaultnav.component.css']
})
export class DefaultnavComponent implements OnInit {

  username = '';
  password = '';
  constructor( public dialog: MatDialog) { }
  openDialog(): void {
    const dialogRef = this.dialog.open(LoginComponent, {
      width: '250px',
      data: {user: this.username, pass: this.password}
    });
  }

  ngOnInit() {
  }

}
@Component({
  selector: 'app-dialog-login',
  templateUrl: '../../home/login/login.component.html',
  styleUrls: ['../../home/login/login.component.css']
})
export class LogindialogComponent {

  constructor(
    public dialogRef: MatDialogRef<LogindialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
