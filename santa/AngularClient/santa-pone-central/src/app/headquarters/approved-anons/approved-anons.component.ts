import { Component, OnInit } from '@angular/core';
import { Anon } from '../../../interfaces/anon';
import { Address } from '../../../interfaces/address';
import { SantaApiService } from '../../services/SantaApiService.service';
import { LoaderService } from '../../services/loader.service';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public SantaApi: SantaApiService, public Loader: LoaderService) { }
  approvedClients: any = [];

  ngOnInit() {
    this.SantaApi.getAllClients().subscribe((data: {}) => {
      console.log(data);
      this.approvedClients = data;
    });
  }
}
