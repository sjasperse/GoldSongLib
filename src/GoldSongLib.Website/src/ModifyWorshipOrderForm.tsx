import { MouseEvent } from 'react';
import { Formik, FormikContext, FormikErrors, FormikProps, useField, useFormik, useFormikContext, validateYupSchema } from 'formik';
import _ from 'lodash';
import { WorshipOrder } from './types';
import DatePicker from 'react-datepicker';

export type ModifyWorshipOrderFormParams = {
  origWorshipOrder: WorshipOrder,
  onSubmit?: (worshipOrder: WorshipOrder) => Promise<void>
}

export default function ModifyWorshipOrderForm({ origWorshipOrder, onSubmit }: ModifyWorshipOrderFormParams
) {
  return (
    <Formik
      initialValues={origWorshipOrder}
      onSubmit={() => { }}
    >
      {Form}
    </Formik>
  );
}

function Form(formik: FormikProps<WorshipOrder>) {
  const onAddSongSet = (e: MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();

    formik.setFieldValue('songSets', [
      ...formik.values.songSets,
      { title: '', songs: [] }
    ])
  }

  const fieldPropsFor = (name: string) => ({
    name: name,
    value: _.get(formik.values, name),
    onChange: formik.handleChange,
    onBlur: formik.handleChange
  });

  return (
    <form onSubmit={formik.handleSubmit}>
      <div>
        <label>Date</label>
        <DatePickerField name="date" />
      </div>

      <div>
        <label>Song Sets</label>
        <ul>
          {formik.values.songSets.map((x, i) => <li key={i}>
            <fieldset>
              <legend>{x.title ?? 'Untitled'}</legend>
              <input type="text" {...fieldPropsFor(`songSets[${i}].title`)} />
            </fieldset>
          </li>)}
        </ul>
        <button onClick={onAddSongSet}>Add Song Set</button>
      </div>


      <pre>
        {JSON.stringify(formik.values, null, 2)}
      </pre>
    </form>
  );
}

function DatePickerField({ name, ...props }: {
  name: string
}) {
  const { setFieldValue } = useFormikContext();
  const [field] = useField({ name, ...props });
  return (
    <DatePicker
      {...field}
      {...props}
      selected={(field.value && new Date(field.value)) || null}
      onChange={(val: Date) => {
        setFieldValue(field.name, val);
      }}
    />
  );
}