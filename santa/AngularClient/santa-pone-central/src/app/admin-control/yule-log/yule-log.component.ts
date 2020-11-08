import { Component, Input, OnInit } from '@angular/core';
import { GathererService } from 'src/app/services/gatherer.service';
import { YuleLogService } from 'src/app/services/santa-api.service';
import { Category, YuleLog } from '../../../classes/yuleLogTypes'

@Component({
  selector: 'app-yule-log',
  templateUrl: './yule-log.component.html',
  styleUrls: ['./yule-log.component.css']
})
export class YuleLogComponent implements OnInit {

  constructor(public YuleLogService: YuleLogService, public gatherer: GathererService) { }

  @Input() allCategories: Array<Category> = [];
  @Input() allYuleLogs: Array<YuleLog> = [];

  async ngOnInit() {
    await this.gatherer.gatherAllYuleLogs();
  }

}
