import { HttpRequest } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { APIResponse, DataResponse, environment, mSettings } from '../../model';
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

  role!: string | undefined;

  get IsAdmin() {
    return this.role == "admin";
  }

  ngOnInit(): void {
  }

}
