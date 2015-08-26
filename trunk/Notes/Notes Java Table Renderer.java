/*Notes Java Table Renderer
 *
 */

import javax.swing.*;
import java.awt.*;
import java.awt.Event.*;
import javax.swing.table.*;
import java.awt.TextComponent;

/*DefaultTableRenderer: 
 * - already implements TableCellRenderer
 * - has additional methods and variables 
 * - http://java.sun.com/j2se/1.4.2/docs/api/javax/swing/table/DefaultTableCellRenderer.html
 */

class colorMe extends DefaultTableCellRenderer { 

       colorMe() {
           setHorizontalAlignment(SwingConstants.RIGHT);

       }
    protected void setValue(Object value){ //use this to set cell's value
        if (value instanceof String){ //based on any condition
            setText((String)value);
            
            setVerticalTextPosition(SwingConstants.CENTER);
            setForeground(Color.red);
        
        }
        else {
            super.setValue(value); //or just pass the it back up...
        }
    }
        

   
}
/*TableCellRenderer:
 * - base interface by which cell rendering occurs
 * - only method id getTableCellRendererComponent
 * - base implementor must extend a component 
 * - http://java.sun.com/j2se/1.4.2/docs/api/javax/swing/table/TableCellRenderer.html
 */
class anotherColorMe extends JLabel implements TableCellRenderer{
    public Component getTableCellRendererComponent(JTable jt, //only method
                Object value, boolean isSelected,
                boolean hasFocus, int row, int column) {
        setText((String)value); //control the text
        if (value instanceof String) {
            setForeground(Color.green);
        }
        return this; //returns an extended JLabel
    }
}
/*TableCellRenderer (cascading)
 * - used to gain cell-by-cell control of the table
 */
class extendMyColor implements TableCellRenderer {
   TableCellRenderer ren; 
   Component comp;
    public extendMyColor(TableCellRenderer ren){ //HAS-A ref to another renderer
        this.ren = ren;
    }
    public Component getTableCellRendererComponent (JTable jt,
            Object value, boolean isSelected, 
            boolean hasFocus, int row, int column){
        comp = ren.getTableCellRendererComponent(jt, value, 
                isSelected, hasFocus, row, column); //use ref to make the component
        
        if (value.toString().endsWith("/")) { //then adjust as needed
            comp.setForeground(Color.orange);
        } else {
            comp.setForeground(Color.green);
        }
        return comp;
    }
}
/*AbstractTableModel:
 * - base to be extended to setup a table
 * - has a handful of 'must override' method
 */
class myRender extends AbstractTableModel  {
    int rowCount; //standard row and column varaibles
    int columnCount;
    
        Object [][] myArray = {  //here is your table data
            {"The Misfits", "Hybrid Moments/", new Boolean(true)},
            {"The Ramones", "Judy Is A Punk", new Boolean(true)},
            {"The Clash", "Jail Guitar Doors", new Boolean(true)},
            {"New York Dolls", "Looking For A Kiss", new Boolean(true)}
        };
        String [] myColumns = {"Band", "Track","Rock?"}; //here is the column names
    public myRender(int row, int col){ //basic constructor
        rowCount = row;
        columnCount = col;
    }//constructor
    
   
    
    public String getColumnName(int col){
        return myColumns[col];
    }
    //MUST OVERRIDE
    public int getRowCount(){
        return rowCount;
    }
    //MUST OVERRIDE
    public int getColumnCount() {
        return columnCount;
    }
    //MUST OVERRIDE
    public Object getValueAt(int row, int col){
        return myArray[row][col];
    }
}

public class MyTurn extends JFrame{
    public MyTurn() {
        super("MyTurn");
        JPanel myPanel = new JPanel();

        getContentPane().add(myPanel);
        Object [][] myArray = {  //another approach to table constructor
            {"The Misfits", "Hybrid Moments/", new Boolean(true)}, 
            {"The Ramones", "Judy Is A Punk", new Boolean(true)},
            {"The Clash", "Jail Guitar Doors", new Boolean(true)},
            {"New York Dolls", "Looking For A Kiss", new Boolean(true)}
        };
        String [] myColumns = {"Band", "Track","Rock?"};

        JTable myTable = new JTable(new myRender(4, 3));//here is what counts

        JScrollPane sp = new JScrollPane(myTable);

        
        myTable.setPreferredScrollableViewportSize(new Dimension(250, 170));
        myPanel.add(sp);
        setSize(300,200);
        setVisible(true);
        
        /*The following is used to set the renderer 
         *on a per-column basis.  To get cell-by-cell
         *renderering you have to cascade the upper renderer
         */
        TableColumnModel tcm = myTable.getColumnModel();
        colorMe myColor = new colorMe();
        anotherColorMe anc = new anotherColorMe();
        extendMyColor extd = new extendMyColor(anc);
        tcm.getColumn(0).setCellRenderer(myColor);
        tcm.getColumn(1).setCellRenderer(extd);
        TableUtilities utl = new TableUtilities();
        
        //always want this or the app will continue to run, just not viewable
        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
    }
    
 
  public static void main(String args[]) {
            new MyTurn().setVisible(true);
    }


}
