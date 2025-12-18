import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Client } from '../models/client.model';
import { Vehicle } from '../models/vehicle.model';

export interface ExportFilters {
  datumOd?: string;
  datumDo?: string;
  dobavljacId?: number | null;
  voziloId?: number | null;
}

@Component({
  selector: 'app-export-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './export-dialog.component.html',
  styleUrl: './export-dialog.component.css'
})
export class ExportDialogComponent {
  @Input() clients: Client[] = [];
  @Input() vehicles: Vehicle[] = [];
  @Output() export = new EventEmitter<ExportFilters>();
  @Output() cancel = new EventEmitter<void>();

  filters: ExportFilters = {
    datumOd: undefined,
    datumDo: undefined,
    dobavljacId: null,
    voziloId: null
  };

  onExport(): void {
    this.export.emit(this.filters);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
