import { Save, X } from 'lucide-react';

function getInitialValue(field) {
  if (field.type === 'number' || field.type === 'currency') {
    return '';
  }

  return '';
}

function normalizeValue(value, field) {
  if (field.type === 'number' || field.type === 'currency') {
    return value === '' ? 0 : Number(value);
  }

  return value;
}

export function createInitialForm(fields) {
  return fields.reduce((form, field) => {
    form[field.name] = getInitialValue(field);
    return form;
  }, {});
}

export function pickFormValues(source, fields) {
  return fields.reduce((form, field) => {
    form[field.name] = source?.[field.name] ?? getInitialValue(field);
    return form;
  }, {});
}

export function EntityForm({ title, fields, value, editing, onChange, onSubmit, onCancel, submitting }) {
  function handleChange(field, nextValue) {
    onChange({
      ...value,
      [field.name]: normalizeValue(nextValue, field)
    });
  }

  return (
    <form className="entity-form" onSubmit={onSubmit}>
      <div className="form-head">
        <h2>{title}</h2>
        {editing && (
          <button className="icon-button" type="button" onClick={onCancel} aria-label="Vazgeç">
            <X size={18} />
          </button>
        )}
      </div>

      <div className="form-grid">
        {fields.map((field) => (
          <label key={field.name} className="field">
            <span>{field.label}</span>
            <input
              type={field.inputType ?? (field.type === 'datetime' ? 'datetime-local' : field.type === 'currency' || field.type === 'number' ? 'number' : 'text')}
              min={field.min}
              step={field.step}
              value={value[field.name] ?? ''}
              onChange={(event) => handleChange(field, event.target.value)}
              placeholder={field.placeholder}
              required
            />
          </label>
        ))}
      </div>

      <button className="primary-button" type="submit" disabled={submitting}>
        <Save size={18} />
        <span>{submitting ? 'Kaydediliyor' : editing ? 'Güncelle' : 'Kaydet'}</span>
      </button>
    </form>
  );
}
