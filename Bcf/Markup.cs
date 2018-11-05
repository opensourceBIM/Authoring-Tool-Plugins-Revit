using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Bimbot.Bcf
{
   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]

   public partial class Markup
   {

      private HeaderFile[] headerField;

      private Topic topicField;

      private Comment[] commentField;

      private ViewPoint[] viewpointsField;

      /// <remarks/>
      [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      [System.Xml.Serialization.XmlArrayItemAttribute("File", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
      public HeaderFile[] Header
      {
         get
         {
            return this.headerField;
         }
         set
         {
            this.headerField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public Topic Topic
      {
         get
         {
            return this.topicField;
         }
         set
         {
            this.topicField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute("Comment", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public Comment[] Comment
      {
         get
         {
            return this.commentField;
         }
         set
         {
            this.commentField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute("Viewpoints", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public ViewPoint[] Viewpoints
      {
         get
         {
            return this.viewpointsField;
         }
         set
         {
            this.viewpointsField = value;
         }
      }
   }


   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
   public partial class HeaderFile
   {

      public HeaderFile()
      {
         this.isExternal = true;
      }

      public string Filename { get; set; }
      public System.DateTime Date { get; set; }
      public string Reference { get; set; }
      public string IfcProject { get; set; }
      public string IfcSpatialStructureElement { get; set; }
      public bool isExternal { get; set; }
   }

   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   public partial class ViewPoint
   {

      private string viewpointField;

      private string snapshotField;

      private int indexField;

      private bool indexFieldSpecified;

      private string guidField;

      public ViewPoint()
      {
         viewpointField = null;
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Viewpoint
      {
         get
         {
            return this.viewpointField;
         }
         set
         {
            this.viewpointField = value;
         }
      }

      [XmlIgnore]
      public VisualizationInfo ViewpointRef { get; set; }


      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Snapshot
      {
         get
         {
            return this.snapshotField;
         }
         set
         {
            this.snapshotField = value;
         }
      }

      [XmlIgnore]
      public BitmapImage SnapshotRef { get; set; }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public int Index
      {
         get
         {
            return this.indexField;
         }
         set
         {
            this.indexField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlIgnoreAttribute()]
      public bool IndexSpecified
      {
         get
         {
            return this.indexFieldSpecified;
         }
         set
         {
            this.indexFieldSpecified = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string Guid
      {
         get
         {
            return this.guidField;
         }
         set
         {
            this.guidField = value;
         }
      }
   }

   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   public partial class Comment
   {

      private System.DateTime dateField;

      private string authorField;

      private string comment1Field;

      private CommentViewpoint viewpointField;

      private System.DateTime modifiedDateField;

      private bool modifiedDateFieldSpecified;

      private string modifiedAuthorField;

      private string guidField;

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public System.DateTime Date
      {
         get
         {
            return this.dateField;
         }
         set
         {
            this.dateField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Author
      {
         get
         {
            return this.authorField;
         }
         set
         {
            this.authorField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute("Comment", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Comment1
      {
         get
         {
            return this.comment1Field;
         }
         set
         {
            this.comment1Field = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public CommentViewpoint Viewpoint
      {
         get
         {
            return this.viewpointField;
         }
         set
         {
            this.viewpointField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public System.DateTime ModifiedDate
      {
         get
         {
            return this.modifiedDateField;
         }
         set
         {
            this.modifiedDateField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlIgnoreAttribute()]
      public bool ModifiedDateSpecified
      {
         get
         {
            return this.modifiedDateFieldSpecified;
         }
         set
         {
            this.modifiedDateFieldSpecified = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string ModifiedAuthor
      {
         get
         {
            return this.modifiedAuthorField;
         }
         set
         {
            this.modifiedAuthorField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string Guid
      {
         get
         {
            return this.guidField;
         }
         set
         {
            this.guidField = value;
         }
      }
   }

   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
   public partial class CommentViewpoint
   {

      private string guidField;

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string Guid
      {
         get
         {
            return this.guidField;
         }
         set
         {
            this.guidField = value;
         }
      }
   }

   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   public partial class BimSnippet
   {

      private string referenceField;

      private string referenceSchemaField;

      private string snippetTypeField;

      private bool isExternalField;

      public BimSnippet()
      {
         this.isExternalField = false;
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Reference
      {
         get
         {
            return this.referenceField;
         }
         set
         {
            this.referenceField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string ReferenceSchema
      {
         get
         {
            return this.referenceSchemaField;
         }
         set
         {
            this.referenceSchemaField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string SnippetType
      {
         get
         {
            return this.snippetTypeField;
         }
         set
         {
            this.snippetTypeField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      [System.ComponentModel.DefaultValueAttribute(false)]
      public bool isExternal
      {
         get
         {
            return this.isExternalField;
         }
         set
         {
            this.isExternalField = value;
         }
      }
   }

   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   public partial class Topic
   {

      private string[] referenceLinkField;

      private string titleField;

      private string priorityField;

      private int indexField;

      private bool indexFieldSpecified;

      private string[] labelsField;

      private System.DateTime creationDateField;

      private string creationAuthorField;

      private System.DateTime modifiedDateField;

      private bool modifiedDateFieldSpecified;

      private string modifiedAuthorField;

      private System.DateTime dueDateField;

      private bool dueDateFieldSpecified;

      private string assignedToField;

      private string stageField;

      private string descriptionField;

      private BimSnippet bimSnippetField;

      private TopicDocumentReference[] documentReferenceField;

      private TopicRelatedTopic[] relatedTopicField;

      private string guidField;

      private string topicTypeField;

      private string topicStatusField;

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute("ReferenceLink", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string[] ReferenceLink
      {
         get
         {
            return this.referenceLinkField;
         }
         set
         {
            this.referenceLinkField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Title
      {
         get
         {
            return this.titleField;
         }
         set
         {
            this.titleField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Priority
      {
         get
         {
            return this.priorityField;
         }
         set
         {
            this.priorityField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public int Index
      {
         get
         {
            return this.indexField;
         }
         set
         {
            this.indexField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlIgnoreAttribute()]
      public bool IndexSpecified
      {
         get
         {
            return this.indexFieldSpecified;
         }
         set
         {
            this.indexFieldSpecified = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute("Labels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string[] Labels
      {
         get
         {
            return this.labelsField;
         }
         set
         {
            this.labelsField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public System.DateTime CreationDate
      {
         get
         {
            return this.creationDateField;
         }
         set
         {
            this.creationDateField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string CreationAuthor
      {
         get
         {
            return this.creationAuthorField;
         }
         set
         {
            this.creationAuthorField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public System.DateTime ModifiedDate
      {
         get
         {
            return this.modifiedDateField;
         }
         set
         {
            this.modifiedDateField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlIgnoreAttribute()]
      public bool ModifiedDateSpecified
      {
         get
         {
            return this.modifiedDateFieldSpecified;
         }
         set
         {
            this.modifiedDateFieldSpecified = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string ModifiedAuthor
      {
         get
         {
            return this.modifiedAuthorField;
         }
         set
         {
            this.modifiedAuthorField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public System.DateTime DueDate
      {
         get
         {
            return this.dueDateField;
         }
         set
         {
            this.dueDateField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlIgnoreAttribute()]
      public bool DueDateSpecified
      {
         get
         {
            return this.dueDateFieldSpecified;
         }
         set
         {
            this.dueDateFieldSpecified = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string AssignedTo
      {
         get
         {
            return this.assignedToField;
         }
         set
         {
            this.assignedToField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Stage
      {
         get
         {
            return this.stageField;
         }
         set
         {
            this.stageField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Description
      {
         get
         {
            return this.descriptionField;
         }
         set
         {
            this.descriptionField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public BimSnippet BimSnippet
      {
         get
         {
            return this.bimSnippetField;
         }
         set
         {
            this.bimSnippetField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute("DocumentReference", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public TopicDocumentReference[] DocumentReference
      {
         get
         {
            return this.documentReferenceField;
         }
         set
         {
            this.documentReferenceField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute("RelatedTopic", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public TopicRelatedTopic[] RelatedTopic
      {
         get
         {
            return this.relatedTopicField;
         }
         set
         {
            this.relatedTopicField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string Guid
      {
         get
         {
            return this.guidField;
         }
         set
         {
            this.guidField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string TopicType
      {
         get
         {
            return this.topicTypeField;
         }
         set
         {
            this.topicTypeField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string TopicStatus
      {
         get
         {
            return this.topicStatusField;
         }
         set
         {
            this.topicStatusField = value;
         }
      }
   }

   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
   public partial class TopicDocumentReference
   {

      private string referencedDocumentField;

      private string descriptionField;

      private string guidField;

      private bool isExternalField;

      public TopicDocumentReference()
      {
         this.isExternalField = false;
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string ReferencedDocument
      {
         get
         {
            return this.referencedDocumentField;
         }
         set
         {
            this.referencedDocumentField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
      public string Description
      {
         get
         {
            return this.descriptionField;
         }
         set
         {
            this.descriptionField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string Guid
      {
         get
         {
            return this.guidField;
         }
         set
         {
            this.guidField = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      [System.ComponentModel.DefaultValueAttribute(false)]
      public bool isExternal
      {
         get
         {
            return this.isExternalField;
         }
         set
         {
            this.isExternalField = value;
         }
      }
   }

   /// <remarks/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
   public partial class TopicRelatedTopic
   {

      private string guidField;

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute()]
      public string Guid
      {
         get
         {
            return this.guidField;
         }
         set
         {
            this.guidField = value;
         }
      }
   }


   /*
   public class Markup
   {
      [System.Xml.Serialization.XmlArrayItemAttribute("File")]
      public HeaderFile Header { get; set; }
      public Topic Topic { get; set; }
      public List<CommentType> Comment { get; set; }
      [XmlElementAttribute]
      public List<ViewPoint> Viewpoints { get; set; }
   }


   public class HeaderFile
   {
      public HeaderFile()
      {
         isExternal = true;
      }

      public string Filename {get; set; }
      public System.DateTime Date { get; set; }
      public string Reference { get; set; }

      public string IfcProject { get; set; }
      public string IfcSpatialStructureElement { get; set; }
      public bool isExternal { get; set; }
   }

   public partial class ViewPoint
   {
      public string Viewpoint { get; set; } //URL
//      public VisualizationInfo ViewpointRef { get; set; }

      public string Snapshot { get; set; } //URL
//      public BitmapImage SnapshotRef { get; set; }

      public int Index { get; set; }

      public string Guid { get; set; }  
   }

   public class BimSnippet
   {
      public string Reference { get; set; }
      public string ReferenceSchema { get; set; }

      public string SnippetType { get; set; }
      public bool isExternal { get; set; }
   }

   public class Topic
   {
      public string[] ReferenceLink { get; set; }
      public string Title { get; set; }
      public string Priority { get; set; }
      public int Index { get; set; }
      public string[] Labels { get; set; }
      public System.DateTime CreationDate { get; set; }
      public string CreationAuthor { get; set; }
      public System.DateTime ModifiedDate { get; set; }
      public string ModifiedAuthor { get; set; }
      public System.DateTime DueDate { get; set; }
      public string AssignedTo { get; set; }
      public string Stage { get; set; }
      public string Description { get; set; }
      public BimSnippet BimSnippet { get; set; }
      public DocumentReference[] DocumentReference { get; set; }
      public RelatedTopic[] RelatedTopic { get; set; }

      public string Guid { get; set; }
      public string TopicType { get; set; }
      public string TopicStatus { get; set; }

   }

   public class DocumentReference
   {
      public DocumentReference()
      {
         isExternal = false;
      }

      public string ReferencedDocument { get; set; }
      public string Description { get; set; }

      public string Guid { get; set; }
      public bool isExternal { get; set; }
   }


   public class RelatedTopic
   {
      public string Guid { get; set; }
   }

   public class CommentType
   {
      public System.DateTime Date { get; set; }
      public string Author { get; set; }
      public string Comment { get; set; }
      public CommentViewpoint Viewpoint { get; set; }
      public System.DateTime ModifiedDate { get; set; }
      public string ModifiedAuthor { get; set; }

      public string Guid { get; set; }
   }

   public class CommentViewpoint
   {
      public string Guid { get; set; }
   }


*/

}
