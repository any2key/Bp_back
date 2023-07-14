import { HttpRequest } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AdminSettings, APIResponse, CryptoSettings, DataResponse, environment, mSettings, PaymentSettings } from '../../model';
import { ApiService } from '../../services/api.service';
import { TokenService } from '../../services/token.service';
import { UiService } from '../../services/ui.service';


@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  constructor(private api: ApiService, private ui: UiService, private http: HttpClient, private token: TokenService) {
    this.role = token.getSession()?.userRole;
  }


  ps!: PaymentSettings;

  selectedFile!: File;
  role!: string | undefined;

  get IsAdmin() {
    return this.role == "admin";
  }

  
  ngOnInit(): void {
    this.api.getData<DataResponse<mSettings<AdminSettings>>>(`settings/Sys`).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      else {
        this.AdminSettings.get('CrmUrl')?.patchValue(res.data.value.crmUrl, { emitEvent: false, onlySelf: true });
        this.AdminSettings.get('Token')?.patchValue(res.data.value.token, { emitEvent: false, onlySelf: true });
        this.AdminSettings.get('ApiKey')?.patchValue(res.data.value.apiKey, { emitEvent: false, onlySelf: true });
        this.AdminSettings.get('ApiTgServiceUrl')?.patchValue(res.data.value.apiTgServiceUrl, { emitEvent: false, onlySelf: true });
        this.AdminSettings.get('GreetingTgMsg')?.patchValue(res.data.value.greetingTgMsg, { emitEvent: false, onlySelf: true });
        this.AdminSettings.get('GlobalTgMsg')?.patchValue(res.data.value.globalTgMsg, { emitEvent: false, onlySelf: true });
        this.AdminSettings.get('LogKeepDays')?.patchValue(res.data.value.logKeepDays, { emitEvent: false, onlySelf: true });
      }
    });

    this.api.getData<DataResponse<mSettings<PaymentSettings>>>(`settings/payment`).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      else {
        this.PaymentSettings.get('ShopId')?.patchValue(res.data.value.shopId, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('ShopKey')?.patchValue(res.data.value.shopKey, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('EmailReciever')?.patchValue(res.data.value.emailReciever, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('EmailSender')?.patchValue(res.data.value.emailSender, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('SenderName')?.patchValue(res.data.value.senderName, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('SmtpServer')?.patchValue(res.data.value.smtpServer, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('SmtpPort')?.patchValue(res.data.value.smtpPort, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('SmtpUser')?.patchValue(res.data.value.smtpUser, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('SmtpPassword')?.patchValue(res.data.value.smtpPassword, { emitEvent: false, onlySelf: true });
        this.PaymentSettings.get('SmtpSSL')?.patchValue(res.data.value.smtpSSL, { emitEvent: false, onlySelf: true });
        this.ps = res.data.value;
      }
    });


    this.AdminSettings.valueChanges.subscribe(r => {
      console.log(r);
      let req = new mSettings<AdminSettings>();
      req.key = 'sys';
      req.value = r;

      this.api.postData<APIResponse, mSettings<AdminSettings>>('settings/savesys', req).subscribe(res => {
        if (!res.isOk)
          this.ui.show(res.message);
      });
    });

    this.PaymentSettings.valueChanges.subscribe(r => {
      let req = new mSettings<PaymentSettings>();
      req.key = "payment";
      req.value = r;
      req.value.lastCheck = this.ps.lastCheck;

      this.api.postData<APIResponse, mSettings<PaymentSettings>>('settings/SavePayment', req).subscribe(res => {
        if (!res.isOk)
          this.ui.show(res.message);
      });

    });

  }


  sendGlobal() {
    this.api.getData<APIResponse>(`settings/GlobalMsg?msg=${this.AdminSettings.get('GlobalTgMsg')?.value}`).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      else this.ui.show("Успешно");
    });
  }

  AdminSettings = new FormGroup(
    {
      CrmUrl: new FormControl(''),
      Token: new FormControl(''),
      ApiKey: new FormControl(''),
      GreetingTgMsg: new FormControl(''),
      GlobalTgMsg: new FormControl(''),
      LogKeepDays: new FormControl(''),
      ApiTgServiceUrl: new FormControl('')
    });

  PaymentSettings = new FormGroup(
    {
      ShopId: new FormControl('0'),
      ShopKey: new FormControl('0'),
      EmailReciever: new FormControl('reciever@mail.com'),
      EmailSender: new FormControl('sender@mail.com'),
      SenderName: new FormControl('name'),
      SmtpServer: new FormControl('server'),
      SmtpPort: new FormControl(0),
      SmtpUser: new FormControl('user'),
      SmtpPassword: new FormControl('pass'),
      SmtpSSL: new FormControl(true)
    });


  downloadLogs() {
    window.open(`${environment.apiUrl}/api/download/getlogs`);
  }

  SendTelegram = new FormGroup(
    {
      ChatId: new FormControl(''),
      Text: new FormControl(''),
    });
  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
    console.log(this.selectedFile.name);
  }

  sendText() {
    this.api.postData<APIResponse, any>(`tgintegro/sendText`, { Data: this.SendTelegram.get('Text')?.value, ChatId: this.SendTelegram.get('ChatId')?.value }).subscribe(res => {
      console.log(res);
    });
  }

  sendFile() {

    const formData = new FormData();
    formData.append('ChatId', this.SendTelegram.get('ChatId')?.value);
    formData.append('Text', this.SendTelegram.get('Text')?.value);
    formData.append('Data', this.selectedFile);

    this.http.post(`${environment.apiUrl}/api/tgintegro/sendfile`, formData).subscribe(res => {
      console.log(res);
    });
  }


  //CryptoSettings = new FormGroup(
  //  {
  //    PubKey: new FormControl(''),
  //    PrivKey: new FormControl('')
  //  });


  async openswagger() {
    window.open(`${environment.apiUrl}/swagger`);
  }

}
