import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AddOrUpdate, APIResponse, DataResponse, Hub } from '../../model';
import { ApiService } from '../../services/api.service';
import { UiService } from '../../services/ui.service';
import { AddHubComponent } from './add-hub/add-hub.component';

@Component({
  selector: 'app-hubs',
  templateUrl: './hubs.component.html',
  styleUrls: ['./hubs.component.css']
})
export class HubsComponent implements OnInit {

  displayedColumns: string[] = ['active','name', 'url', 'servers', 'action'];
  dataSource = new MatTableDataSource<Hub>();
  @ViewChild(MatPaginator)
  paginator!: MatPaginator | null;
  @ViewChild(MatSort)
  sort!: MatSort | null;
  constructor(private api: ApiService, private ui: UiService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.refreshTable();
  }

  refreshTable()
  {
    this.api.getData<DataResponse<Hub[]>>('hub/hubs').subscribe(res =>
    {
      if (res.isOk) {
        this.dataSource = new MatTableDataSource<Hub>(res.data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
      else this.ui.show(res.message);
    });
  }

  openDialog(sign: AddOrUpdate, hub: Hub = null) {
    const data: Hub = hub;
    const dialogRef = this.dialog.open(AddHubComponent, {
      width: '400px',
      data: data,
    });
    dialogRef.afterClosed().subscribe(result => {
      if (!result.isOk || result == undefined || result == null)
        return;
      else {
        this.ui.show("Успешно");
        this.refreshTable();
      }
    });

  }

  delete(id: string) {

  }

  addServer(id: string) {
    this.api.getData<APIResponse>(`hub/addServer?id=${id}`).subscribe(res =>
    {
      if (!res.isOk)
        this.ui.show(res.message);
      else this.ui.show('Успешно');

      this.refreshTable();
    });
  }

}
