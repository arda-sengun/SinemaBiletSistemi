import { AlertTriangle, X } from 'lucide-react';

export function ConfirmDialog({ open, title, description, confirmText = 'Onayla', cancelText = 'Vazgeç', onConfirm, onCancel }) {
  if (!open) {
    return null;
  }

  return (
    <div className="modal-backdrop" role="presentation">
      <section className="confirm-dialog" role="dialog" aria-modal="true" aria-labelledby="confirm-title">
        <button className="icon-button modal-close" type="button" onClick={onCancel} aria-label="Kapat">
          <X size={18} />
        </button>

        <div className="confirm-icon">
          <AlertTriangle size={24} />
        </div>

        <div className="confirm-copy">
          <h2 id="confirm-title">{title}</h2>
          <p>{description}</p>
        </div>

        <div className="confirm-actions">
          <button className="secondary-button" type="button" onClick={onCancel}>
            {cancelText}
          </button>
          <button className="danger-button" type="button" onClick={onConfirm}>
            {confirmText}
          </button>
        </div>
      </section>
    </div>
  );
}
