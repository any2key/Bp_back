import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { AdminSettings, CrmClient, DataResponse, environment, Key, KeyHistoryUI, mSettings } from '../../model';
import { ApiService } from '../../services/api.service';
import { UiService } from '../../services/ui.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  displayedColumns: string[] = ['changed', 'key', 'crmTaskId', 'user', 'chDate', 'client', 'comment'];
  dataSource = new MatTableDataSource<KeyHistoryUI>();
  @ViewChild(MatPaginator)
  paginator!: MatPaginator | null;
  @ViewChild(MatSort)
  sort!: MatSort | null;


  constructor(private api: ApiService, private ui: UiService, private router: Router) { }

  keys: Key[] = [];
  total: number = 0;
  infinite: number = 0;
  subscribed: number = 0;
  isLoaded = false;
  ngOnInit(): void {
    this.api.getData<DataResponse<Key[]>>('admin/dash').subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      else {
        var infinDate = new Date('01.01.2500');
        console.log(infinDate);
        this.keys = res.data;
        this.total = this.keys.length;
        this.infinite = this.keys.filter(e => new Date(e.expired) >= infinDate).length;
        this.subscribed = this.keys.filter(e => new Date(e.expired) < infinDate).length
      }
    });

    this.refreshTable();
   
  }


 

  refreshTable() {
    this.isLoaded = true;
    this.api.getData<DataResponse<KeyHistoryUI[]>>(`admin/dashhistory`).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      else {
        this.dataSource = new MatTableDataSource<KeyHistoryUI>(res.data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }

      this.isLoaded = false;
    });
  }

  goToKey(key: string) {
    this.router.navigate([`admin`, `key`, `${key}`]);
  }


  diffDate(d1: Date, d2: Date) {
    let date1 = new Date(d1);
    let date2 = new Date(d2);

    var diff = (date1.getTime() - date2.getTime());
    var diffDays = Math.ceil(diff / (1000 * 3600 * 24));
    return diffDays;
  }

  IsInfin(d1: Date, d2: Date) {
    var infinDate = new Date('01.01.2500');
    let date1 = new Date(d1);
    let date2 = new Date(d2);

    return infinDate.getTime() == date1.getTime() || infinDate.getTime() == date2.getTime();
  }

  goToKeys(Type: number) {
    this.router.navigate([`admin`, `keysDash`, `${Type}`]);
  }
  linkClient(client: CrmClient) {
    this.api.getData<DataResponse<mSettings<AdminSettings>>>(`settings/Sys`).subscribe(res =>
    {
      window.open(`${res.data.value.crmUrl}/contact/${client.id}`, '_blank');
    });
  }

  linkTask(task: string) {
    if (task.length == 0)
      return;
    this.api.getData<DataResponse<mSettings<AdminSettings>>>(`settings/Sys`).subscribe(res => {
      window.open(`${res.data.value.crmUrl}/task/${task}`, '_blank');
    });
  }
}
