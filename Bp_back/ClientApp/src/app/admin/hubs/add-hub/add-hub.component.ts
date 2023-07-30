import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { APIResponse, Hub } from '../../../model';
import { ApiService } from '../../../services/api.service';
import { UiService } from '../../../services/ui.service';

@Component({
  selector: 'app-add-hub',
  templateUrl: './add-hub.component.html',
  styleUrls: ['./add-hub.component.css']
})
export class AddHubComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<AddHubComponent>, private ui: UiService, private api: ApiService,
    @Inject(MAT_DIALOG_DATA) public data: Hub) { }
  header!: string;


  ngOnInit(): void {
    this.header = this.data == null ? 'Добавить хаб' : 'Редактировать хаб';
    this.hub.patchValue(this.data);
  }

  hub = new FormGroup(
    {
      name: new FormControl('', [Validators.required]),
      url: new FormControl('', [Validators.required]),
      port: new FormControl('', [Validators.required]),
    });

  onNoClick(): void {
    const result: any = { isOk: false };
    this.dialogRef.close(result);
  }

  apply() {

    if (!this.hub.valid) {
      this.ui.show("Форма заполнена не корректно");
      return;
    }
    const p: Hub = this.hub.value;
    p.id = this.data != null ? this.data.id : null;
    const result = { isOk: true, data: p };
    result.isOk = true;
    result.data = p;

    if (this.data == null)
      this.api.getData<APIResponse>(`hub/addhub?url=${this.hub.get('url').value}&port=${this.hub.get('port').value}&name=${this.hub.get('name').value}`,).subscribe(res => {
        if (!res.isOk) {
          this.ui.show(res.message);
        } else {
          this.dialogRef.close(result);
        }
      });
    else
      this.api.postData<APIResponse, Hub>(`hub/updateHub`, p).subscribe(res => {
        if (!res.isOk) {
          this.ui.show(res.message);
        } else {
          this.dialogRef.close(result);
        }
      });
  }

}
