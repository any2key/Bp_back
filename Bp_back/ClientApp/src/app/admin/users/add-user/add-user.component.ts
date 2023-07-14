import { AfterViewInit, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { APIResponse, User } from '../../../model';
import { ApiService } from '../../../services/api.service';
import { TokenService } from '../../../services/token.service';
import { UiService } from '../../../services/ui.service';



@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit, AfterViewInit {
  @ViewChild('btnRef')
    buttonRef!: MatButton;
  constructor(
    public dialogRef: MatDialogRef<AddUserComponent>, private ui: UiService, private token: TokenService, private api: ApiService,
    @Inject(MAT_DIALOG_DATA) public data: User) { }
    ngAfterViewInit(): void {
      this.buttonRef?.focus();
    }

  header!: string;
  admin: string = "admin";
  suser: string = "user";
  smuser: string = "smuser";
  tguser: string = "tguser";
  show = false;

  ngOnInit(): void {
    this.header = 'Редактор профиля';
    this.data.password = "";
    console.log(this.data);
    this.user.patchValue(this.data);

    this.user.valueChanges.subscribe(res =>
    {
     // this.buttonRef?.focus();

    });
  }


  generatePass() {
    this.user.get('password')?.reset();
    this.user.get('password')?.patchValue(Array(8).fill("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~!@-#$").map(function (x) { return x[Math.floor(Math.random() * x.length)] }).join(''));
    ;
  }

  keyUpFunction(res: any) {
    this.apply();
  }

  changeShow() {
    this.show = !this.show;
  }
  user = new FormGroup(
    {
      role: new FormControl('', [Validators.required]),
      login: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.email]),
      password: new FormControl(''),
    });

  onNoClick(): void {
    const result: any = { isOk:false };
    this.dialogRef.close(result);
  }

  apply() {

    if (!this.user.valid) {
      this.ui.show("Форма заполнена не корректно");
      return;
    }
    const p: User = this.user.value;
    p.id = this.data.id;
    p.passwordsalt = '';
    p.refreshtokens = [];
    p.active = this.data.active;
    const result = {isOk:true,data:p};
    result.isOk = true;
    result.data = p;

    this.token.checkToken();
    this.api.postData<APIResponse, User>('admin/AddOrUpdateUser', p).subscribe(res => {
      if (!res.isOk) {
        this.ui.show(res.message);
      } else {
    this.dialogRef.close(result);
      }
    });





  }
}
