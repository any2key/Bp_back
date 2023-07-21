import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Dashboard, DataResponse, environment, mSettings } from '../../model';
import { ApiService } from '../../services/api.service';
import { UiService } from '../../services/ui.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(private api: ApiService, private ui: UiService, private router: Router) { }



  dashboard: Dashboard;

  isLoaded = false;
  ngOnInit(): void {
    this.isLoaded = true;
    this.api.getData<DataResponse<Dashboard>>('dashboard/hubs').subscribe(res => {
      if (res.isOk)
        this.dashboard = res.data;
      else
        this.ui.show(res.message);
      this.isLoaded = false;
    }, () => { this.isLoaded = false });
  }


  goTo(route: string) {
    this.router.navigate(['admin',route]);
  }
}
