import javax.swing.*;
/*Notes Java Table Examples*/

/* "Java™ Design Patterns: A Tutorial" 
by James W. Cooper 
Publisher: Addison Wesley Professional 
Pub Date: February 03, 2000 
Print ISBN-10: 0-201-48539-7 
Print ISBN-13: 978-0-201-48539-4
Chapter 32 
*/
class SimpleTable extends JFrame { //a top level from for app start
    public SimpleTable() {          //the constructor
        super("Simple table");        //call to super
        JPanel jp = new JPanel();     
        getContentPane().add(jp);
        Object[] [] musicData = {    //two dim Obj array 
            {"Tschaikovsky", "1812 Overture", new Boolean(true)},
            {"Stravinsky", "Le Sacre", new Boolean(true)}
            {"Lennon", "Eleanor Rigby", new Boolean(false)}
            {"Wagner", "Gotterdammerung", new Boolean (true)}
        };
        String[] columnNames = {"Composer", "Title",  
                "Orchestral"};  //column titles
        JTable table   = new JTable(musicData, columnNames); //data, column name
        JScrollPane sp = new JScrollPane(table); //no idea what this is doing
        table.setPreferredScrollableViewportSize( 
                new Dimension(250,170));
        jp.add(sp);

        setSize(300,200);
        setVisible(true);
    }

/*-----------------------------------*/
/*To take better control of the table you extend AbstractTableModel 
*  - must Override
*    - public int getRowCount() {}
*    - public int getColumnCount() {}
*    - public Object getValueAt(int row, int column) {}
*  - Override the other methods for even more control
*/
public class MusicModel extends AbstractTableModel {
    //here is your column header
    String[] columnNames = {"Composer", "Title", "Orchestral"};
    //here is the matrix array and the data
    Object[] [] musicData = {
        {"Tschaikovsky", "1812 Overture", new Boolean (true)},
        {"Stravinsky", "Le Sacre", new Boolean(true)},
        {"Lennon", "Eleanor Rigby", new Boolean (false)},
        {"Wagner", "Gotterdammerung", new Boolean(true)}
    };
    private int rowCount, columnCount;
    
    //constructor
    public MusicModel(int rowCnt, int colCnt) {
        rowCount = rowCnt;
        columnCount = colCnt;
    }
    
    public String getColumnName(int col) {
        return columnNames[col];
    }

    //MUST OVERRIDE
    public int getRowCount() {
        return rowCount;
    }
    //MUST OVERRIDE
    public int getColumnCount() {
        return columnCount;
    }

    public class getColumnClass(int col) {
        return getValueAt(0, col).getClass();
    }

    public boolean isCellEditable(int row, int col) {
        return(col > 1);
    }

    public void setValueAt(Object obj, int row, int col) {
        musicData[row][col] = obj;
        fireTableCellUpdated(row, col);
    }
    //MUST OVERRIDE
    public Object getValueAt(int row, int col) {
        return musicData[row][col];
    }
}
//now implement SimpleTable with:
//JTable table = new JTable(new MusicModel(4,3));


    
