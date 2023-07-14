import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AddOrUpdate, APIResponse, DataResponse, User } from '../../model';
import { ApiService } from '../../services/api.service';
import { TokenService } from '../../services/token.service';
import { UiService } from '../../services/ui.service';
import { AddUserComponent } from './add-user/add-user.component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {


  displayedColumns: string[] = ['id', 'login', 'email', 'role', 'active', 'action'];
  dataSource = new MatTableDataSource<User>();
  @ViewChild(MatPaginator)
  paginator!: MatPaginator | null;
  @ViewChild(MatSort)
  sort!: MatSort | null;
  constructor(private api: ApiService, private ui: UiService, public dialog: MatDialog, private token: TokenService) { }

  ngOnInit(): void {
    this.refreshTable();
  }

  refreshTable() {
    this.token.checkToken();
    this.api.getData<DataResponse<User[]>>('admin/fetchUsers').subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      else {
        this.dataSource = new MatTableDataSource<User>(res.data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
    });
  }


  openDialog(sign: AddOrUpdate, user: User = new User()) {
    const data: User = user;
    if (sign == AddOrUpdate.add) {
      data.id = -1;
      data.active = true;
    }

    const dialogRef = this.dialog.open(AddUserComponent, {
      width: '400px',
      data: data,
    });
    dialogRef.afterClosed().subscribe(result => {
      if (!result.isOk || result == undefined || result == null)
        return;
      else {
        this.ui.show("Успешно");
        this.refreshTable();
        //let req: User = new User();
        //req = result.data;
        //this.token.checkToken();
        //this.api.postData<APIResponse, User>('admin/AddOrUpdateUser', req).subscribe(res => {
        //  if (!res.isOk) {
        //    this.ui.show(res.message);
        //  } else {
        //    this.ui.show("Успешно");
        //    this.refreshTable();
        //  }
        //});
      }
    });
  }

  deleteUser(id: number) {

    this.ui.confirmation().subscribe(res =>
    {
      if (res) {
        this.token.checkToken();
        this.api.getData<APIResponse>(`admin/Deleteuser?userId=${id}`).subscribe(res => {
          if (!res.isOk)
            this.ui.show(res.message);
          else this.ui.show("Успешно");

          this.refreshTable();
        });
      }
    })    
  }


  changeStatus(user: User) {
    user.active = !user.active;
    user.refreshtokens = [];
    user.password = '';
    this.token.checkToken();
    this.api.postData<APIResponse, User>('admin/AddOrUpdateUser', user).subscribe(res => {
      if (!res.isOk) {
        this.ui.show(res.message);
      } else {
        this.ui.show("Успешно");
        this.refreshTable();
      }
    });
  }

}
