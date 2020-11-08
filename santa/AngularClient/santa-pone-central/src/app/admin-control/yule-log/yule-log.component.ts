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

  gatheringAllCategories: boolean = false;
  gatheringAllYuleLogs: boolean = false;

  public selectedCategory: Category = new Category();

  async ngOnInit() {
    this.gatherer.gatheringAllCategories.subscribe((status: boolean) => {
      this.gatheringAllCategories = status;
    });
    this.gatherer.gatheringAllYuleLogs.subscribe((status: boolean) => {
      this.gatheringAllYuleLogs = status;
    });
    await this.gatherer.gatherAllCategories();
    await this.gatherer.gatherAllYuleLogs();
  }
  public setSelectedCategory(category: Category)
  {
    if(category != null)
    {
      this.selectedCategory = category;
    }
    else
    {
      this.selectedCategory = new Category();
    }
  }
  public filterLogs() : Array<YuleLog>
  {
    if(this.selectedCategory.categoryID != undefined)
    {
      return this.allYuleLogs.filter((log: YuleLog) => {return log.category.categoryID == this.selectedCategory.categoryID})
    }
    else
    {
      return this.allYuleLogs
    }
  }
}
