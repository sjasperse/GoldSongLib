import { MouseEvent, useContext } from 'react';
import { Formik, FormikContext, FormikErrors, FormikProps, useField, useFormik, useFormikContext, validateYupSchema } from 'formik';
import _ from 'lodash';
import { Song, WorshipOrder, WorshipOrderSong, WorshipOrderSongSet } from '../types';
import DatePicker from 'react-datepicker';
import './WorshipOrderEditorForm.scss';
import { useDrop } from 'react-dnd';
import newGuid from '../services/newGuid';
import { AppContext } from '../AppContext';

export type WorshipOrderEditorFormParams = {
  origWorshipOrder: WorshipOrder,
  onSubmit?: (worshipOrder: WorshipOrder) => Promise<void>
}

type GetFieldProps = (name: string) => {
  name: string,
  value: any,
  onChange: (e: React.ChangeEvent<any>) => void,
  onBlur: (e: React.ChangeEvent<any>) => void
};

type GetSongTitleById = (id: string) => string;

export default function WorshipOrderEditorForm({ origWorshipOrder, onSubmit }:WorshipOrderEditorFormParams
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
  const { songs } = useContext(AppContext);
  const songsById = songs!.reduce((p: { [id: string]: Song }, x) => { p[x.id] = x; return p; }, {});
  const getSongTitle = (songId: string) => songsById[songId].name;

  const onAddSongSet = (e: MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();

    formik.setFieldValue('songSets', [
      ...formik.values.songSets,
      { title: '', songs: [] }
    ])
  }

  const fieldPropsFor: GetFieldProps = (name: string) => ({
    name: name,
    value: _.get(formik.values, name),
    onChange: formik.handleChange,
    onBlur: formik.handleChange
  });

  return (
    <div>
      <div>
        <form onSubmit={formik.handleSubmit}>
          <div>
            <label>Date</label>
            <DatePickerField name="date" />
          </div>

          <div>
            <label>Song Sets</label>
            <ul>
              {formik.values.songSets.map((songSet, songSetIndex) => 
                <SongSetComponent 
                  key={songSetIndex} 
                  name={`songSets[${songSetIndex}]`} 
                  songSet={songSet} 
                  fieldPropsFor={fieldPropsFor}
                  getSongTitle={getSongTitle}
                />)}
            </ul>
            <button onClick={onAddSongSet}>Add Song Set</button>
          </div>


          {/* <pre>
            {JSON.stringify(formik.values, null, 2)}
          </pre> */}
        </form>
      </div>
    </div>
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

function SongSetComponent({ name, songSet, fieldPropsFor, getSongTitle }: { name: string, songSet: WorshipOrderSongSet, fieldPropsFor: GetFieldProps, getSongTitle: GetSongTitleById }) {
  const { values, setFieldValue } = useFormikContext();
  
  const [{ canDrop, isOver}, drop] = useDrop({
    accept: "Song",
    drop: () => ({ onDropped: handleSongDrop}),
    collect: monitor => ({
      isOver: monitor.isOver(),
      canDrop: monitor.canDrop()
    }),
  });

  const dropTargetStyle = {
    minHeight: '2em',
    border: canDrop ? '1px dashed gray' : '',
    backgroundColor: isOver ? '#E7FCFF' : ''
  };

  function handleSongDrop(song: Song) {
    const newSongset: WorshipOrderSongSet = {
      ...songSet,
      songs: [
        ...songSet.songs,
        {
          id: newGuid(),
          songId: song.id,
        }
      ]
    }

    setFieldValue(name, newSongset);
  }

  return <fieldset className="song-set">
    <legend>{songSet.title ?? 'Untitled'}</legend>
    <input type="text" {...fieldPropsFor(`${name}.title`)} />
    <ul>
      {songSet.songs.map((song, songIndex) => 
        <SongComponent 
          key={song.id} 
          name={`${name}.songs[${songIndex}]`} 
          song={song}
          getSongTitle={getSongTitle}
        />)}
    </ul>

    <div ref={drop} style={dropTargetStyle} />
  </fieldset>
}

function SongComponent({ name, song, getSongTitle }: { name: string, song: WorshipOrderSong, getSongTitle: GetSongTitleById }) {
  return <li key={song.songId}>{getSongTitle(song.songId)}</li>
}

export type SongDropResult = {
  onDropped: (song: Song) => void
}
