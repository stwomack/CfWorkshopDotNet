using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using System.Web.Mvc;
using log4net;
using MySql.Data.MySqlClient;
using CfWorkshopDotNet.Models;

namespace CfWorkshopDotNet.Controllers
{
    public class NotesController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NotesController));

        private MySqlConnection mySqlConnection;

        public NotesController(MySqlConnection mySqlConnection)
        {
            this.mySqlConnection = mySqlConnection;
        }

        // GET: Notes
        public ActionResult Index()
        {
            log.Debug("Fetching all notes.");
            mySqlConnection.Open();
            DbCommand command = new MySqlCommand("SELECT * FROM NOTES", mySqlConnection);
            DbDataReader reader = command.ExecuteReader();

            List<Note> notes = new List<Note>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Note note = new Note();
                    note.ID = reader.GetInt32(0);
                    note.Created = reader.GetDateTime(1);
                    note.Text = reader.GetString(2);
                    notes.Add(note);
                }
            }

            reader.Close();
            mySqlConnection.Close();
            log.Debug(string.Format("Notes retrieved:[{0}]", notes));
            return View(notes);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Text,Created")] Note note)
        {
            if (ModelState.IsValid)
            {
                log.Debug(string.Format("Creating Note {0}", note));
                mySqlConnection.Open();
                DbCommand command = new MySqlCommand("INSERT INTO NOTES(ID, TEXT) VALUES (@ID, @TEXT)", mySqlConnection);
                command.Parameters.Add(new MySqlParameter("@ID", note.ID));
                command.Parameters.Add(new MySqlParameter("@TEXT", note.Text));
                command.ExecuteNonQuery();
                mySqlConnection.Close();
                return RedirectToAction("Index");
            }

            return View(note);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            mySqlConnection.Open();
            DbCommand command = new MySqlCommand("SELECT * FROM NOTES WHERE ID = @ID", mySqlConnection);
            command.Parameters.Add(new MySqlParameter("@ID", id));
            DbDataReader reader = command.ExecuteReader();

            Note note;
            if (!reader.HasRows)
            {
                return HttpNotFound();
            }
            else
            {
                reader.Read();
                note = new Note
                {
                    ID = reader.GetInt32(0),
                    Created = reader.GetDateTime(1),
                    Text = reader.GetString(2)
                };
            }

            reader.Close();
            mySqlConnection.Close();

            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Text,Created")] Note note)
        {
            
            if (ModelState.IsValid)
            {
                mySqlConnection.Open();
                DbCommand command = new MySqlCommand("UPDATE NOTES SET CREATED = @CREATED, TEXT = @TEXT WHERE (ID = @ID)", mySqlConnection);
                command.Parameters.Add(new MySqlParameter("@ID", note.ID));
                command.Parameters.Add(new MySqlParameter("@CREATED", note.Created));
                command.Parameters.Add(new MySqlParameter("@TEXT", note.Text));
                command.ExecuteNonQuery();
                mySqlConnection.Close();
                return RedirectToAction("Index");
            }
            return View(note);
        }


        // GET: Notes/Delete/5
        [HttpGet, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            mySqlConnection.Open();
            DbCommand command = new MySqlCommand("DELETE FROM NOTES WHERE (ID = @ID)", mySqlConnection);
            command.Parameters.Add(new MySqlParameter("@ID", id));
            command.ExecuteNonQuery();
            mySqlConnection.Close();
            return RedirectToAction("Index");
        }

    }
}
