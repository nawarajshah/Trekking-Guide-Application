import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RequestService } from '../../service/request.service';
import { ToastrService } from 'ngx-toastr';
import { Request } from '../../service/request.service';

@Component({
  selector: 'app-guide-requests',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './guide-requests.component.html',
  styleUrl: './guide-requests.component.css'
})
export class GuideRequestsComponent implements OnInit{
  requests: Request[] = [];

  constructor(
    private requestService: RequestService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
      this.loadRequests();
  }

  loadRequests(): void {
    this.requestService.getGuideRequests().subscribe({
      next: (data) => {
        this.requests = data;
      },
      error: (err) => {
        this.toastr.error('Error loading guide requests');
      }
    });
  }

  accept(reqId: number) {
    this.requestService.acceptRequest(reqId).subscribe({
      next: (res) => {
        this.toastr.success(res.message);
        this.loadRequests();
      },
      error: (err) => {
        this.toastr.error('Error accepting requests');
        console.error('Error accepting requests: ' + err);
      }
    })
  }

  reject(reqId: number) {
    this.requestService.rejectRequest(reqId).subscribe({
      next: (res) => {
        this.toastr.success(res.message);
        this.loadRequests();
      },
      error: (err) => {
        this.toastr.error('Error rejecting request');
        console.error('Error rejecting: ' + err);
      }
    });
  }

}
