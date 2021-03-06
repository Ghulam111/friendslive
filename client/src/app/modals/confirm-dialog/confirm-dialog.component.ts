import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
  title : string;
  message : string;
  btnOkText : string;
  btnCancelText : string;
  result : boolean;

  constructor(public bsmodalRef : BsModalRef) { }

  ngOnInit(): void {
  }

  confirm()
  {
    this.result = true;
    this.bsmodalRef.hide();
  }

  decline()
  {
    this.result = false;
    this.bsmodalRef.hide();
  }

}
