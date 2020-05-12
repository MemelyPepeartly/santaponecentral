import { Component, OnInit } from '@angular/core';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from '../services/SantaApiService.service';
import { Tag } from 'src/classes/tag';
import { MapService } from '../services/MapService.service';

@Component({
  selector: 'app-admin-control',
  templateUrl: './admin-control.component.html',
  styleUrls: ['./admin-control.component.css']
})
export class AdminControlComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ApiMapper: MapService) { }

  public allTags: Array<Tag> = [];
  
  public tagControlSelected: boolean = false;

  async ngOnInit() {
    await this.gatherAllTags();
  }
  public selectTagControl()
  {
    this.tagControlSelected = true;
  }
  public async refreshTags(event: boolean)
  {
    if(event)
    {
      await this.gatherAllTags();
    }
  }
  public async gatherAllTags()
  {
    this.allTags = [];
    var tagRes = await this.SantaApiGet.getAllTags().toPromise();
    for(let i = 0; i < tagRes.length; i++)
    {
      this.allTags.push(this.ApiMapper.mapTag(tagRes[i]))
    }
  }

}
