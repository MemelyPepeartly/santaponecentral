import { Component, OnInit } from '@angular/core';
import { SantaApiService } from '../../services/SantaApiService.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  constructor(public SantaApi: SantaApiService) { }

  users: any = [];
  ngOnInit()
  {

  }
  getUsers()
  {
    this.SantaApi.getAllClients().subscribe((data: {}) => {
      this.users = data;
    });
  }

}
