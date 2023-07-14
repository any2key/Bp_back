import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ConfirmComponent>) { }

  ngOnInit(): void {
  }


  no(): void {
    this.dialogRef.close(false);
  }

  yes() {
    this.dialogRef.close(true);
  }
}
