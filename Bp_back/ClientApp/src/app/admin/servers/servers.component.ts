import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { APIResponse, DataResponse, Hub, Server, ServerFilter } from '../../model';
import { ApiService } from '../../services/api.service';
import { TokenService } from '../../services/token.service';
import { UiService } from '../../services/ui.service';

@Component({
  selector: 'app-servers',
  templateUrl: './servers.component.html',
  styleUrls: ['./servers.component.css']
})
export class ServersComponent {

  displayedColumns: string[] = ['url', 'port', 'httpPort', 'hub', 'active', 'playersCount', 'action'];
  dataSource = new MatTableDataSource<Server>();
  @ViewChild(MatPaginator)
  paginator!: MatPaginator | null;
  @ViewChild(MatSort)
  sort!: MatSort | null;
  private subscription: Subscription;
  hubs: Hub[] = [];
  isLoaded = false;

  hubId!: string;

  constructor(private api: ApiService, private ui: UiService, private router: Router, private activateRoute: ActivatedRoute, private token: TokenService) {

    this.api.getData<DataResponse<Hub[]>>('hub/hubs').subscribe(res => {
      if (res.isOk) {
        this.hubs = res.data;
      }
      else this.ui.show(res.message);
    });

    this.subscription = activateRoute.params.subscribe(
      (params) => {
        console.log(params);
        (this.hubId = params['Id']);

        if (this.hubId != undefined) {
          this.filter.get('hubId')?.patchValue(this.hubId);
          this.refreshTable();
        } else this.refreshTable();
      });
  }

  filter = new FormGroup(
    {
      hubId: new FormControl(''),
      active: new FormControl('')
    });


  refreshTable() {
    this.isLoaded = true;
    let data: ServerFilter = this.filter.value;

    this.api.postData<DataResponse<Server[]>, ServerFilter>(`server/fetch`, data).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      else {
        this.dataSource = new MatTableDataSource<Server>(res.data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
      this.isLoaded = false;
    });
  }

  filt() {
    this.refreshTable();
  }

  startServer(id: string, port: number) {
    this.api.getData<APIResponse>(`server/StartServer?hub=${id}&port=${port}`).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      this.refreshTable();
    });
  }

  stopServer(id: string, port: number) {
    this.api.getData<APIResponse>(`server/StopServer?hub=${id}&port=${port}`).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      this.refreshTable();

    });
  }
  removeServer(id: string, port: number) {
    this.api.getData<APIResponse>(`server/removeServer?hub=${id}&port=${port}`).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      this.refreshTable();

    });
  }
}
